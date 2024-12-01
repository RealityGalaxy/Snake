const canvas = document.getElementById('gameCanvas');
const context = canvas.getContext('2d');

let cellSize = canvas.width / 30; // Adjusted to fit the grid size
let currentInstance = 0;

// Initialize SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .build();

let manual = false;

function selectManual() {
    if (manual) {
        manual = false;
    }
    else {
        manual = true;
    }
}

connection.on("ReceiveGameState", function (gameState) {
    // Update the canvas with the new game state
    drawGame(gameState);
    updateLeaderboard(gameState);
});

let selectedLevel = 1;  // Default to level 1
document.getElementById('level1').checked = true;

function selectLevel(level) {
    // Uncheck all levels first
    document.getElementById('level1').checked = false;
    document.getElementById('level2').checked = false;
    document.getElementById('level3').checked = false;

    // Check only the selected level
    document.getElementById('level' + level).checked = true;

    // Update the selected level
    selectedLevel = level;
}

// Function to send the selected level to the server when generating the game
function generateGame() {
    connection.invoke("ResetGame", selectedLevel, currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}

function pause() {
    connection.invoke("Pause", currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}

function undo() {
    connection.invoke("Undo", currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}


connection.on("UpdateGlobalLeaderboard", function (topScores) {
    const sortedScores = topScores.sort((a, b) => b.score - a.score);
    console.log(sortedScores);

    const highScores = document.querySelector('.highscores ul');
    if (!highScores) {
        console.error("Highscores element not found. Check your HTML structure.");
        return;
    }

    // Clear the current leaderboard
    highScores.innerHTML = '';

    // Loop through each score and create a new list item for the leaderboard
    sortedScores.forEach((score, index) => {
        const listItem = document.createElement('li');
        listItem.textContent = `${index + 1}. ${score.key} - ${score.value}`;
        highScores.appendChild(listItem);
    });
});


function updateLeaderboard(gameState) {
    const snakes = gameState.snakes;
    const sortedSnakes = snakes.sort((a, b) => b.body.length - a.body.length);

    const leaderboard = document.querySelector('.leaderboard ul');
    if (!leaderboard) {
        console.error("Leaderboard element not found. Check your HTML structure.");
        return;
    }

    // Clear the current leaderboard
    leaderboard.innerHTML = '';

    // Loop through each snake and create a new list item for the leaderboard
    sortedSnakes.forEach((snake, index) => {
        const listItem = document.createElement('li');
        listItem.textContent = `${index + 1}. ${snake.name}`;
        listItem.style.color = snake.color;
        leaderboard.appendChild(listItem);
    });
}



document.addEventListener('keydown', function (event) {
    switch (event.code) {
        case 'ArrowUp':
        case 'KeyW':
            sendDirection('Up');
            break;
        case 'ArrowDown':
        case 'KeyS':
            sendDirection('Down');
            break;
        case 'ArrowLeft':
        case 'KeyA':
            sendDirection('Left');
            break;
        case 'ArrowRight':
        case 'KeyD':
            sendDirection('Right');
            break;
    }
});

connection.start().then(function () {
    console.log("Connected to GameHub");
}).catch(function (err) {
    return console.error(err.toString());
});

// Function to send direction to the server
function sendDirection(direction) {
    connection.invoke("SendDirection", direction, currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}

function joinGame() {
    const playerName = document.getElementById('playerName').value;

    if (playerName) {
        const color = getVibrantRandomColor();

        setButtonColors(color);
        connection.invoke("AddSnake", color, playerName, currentInstance, manual).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        alert("Please enter your name!");
    }
}

const allButtons = document.querySelectorAll('button');

function setButtonColors(color) {
    allButtons.forEach(button => {
        button.style.backgroundColor = color;
    })
}

// List of vibrant preset colors
const vibrantColors = [
    '#FF5733', // Vibrant Red-Orange
    '#FFBD33', // Vibrant Yellow
    '#33FF57', // Vibrant Green
    '#33FFBD', // Vibrant Aqua
    '#33A1FF', // Vibrant Blue
    '#A133FF', // Vibrant Purple
    '#FF33A1', // Vibrant Pink
];

// Function to return a random color from the preset list
let counter = Math.floor(Math.random() * vibrantColors.length);
function getVibrantRandomColor() {
    const randomIndex = counter; // Random index based on the array length
    counter = (counter + 1) % vibrantColors.length;
    return vibrantColors[randomIndex]; // Return a random vibrant color
}


// Function to draw the game state
function drawGame(gameState) {
    cellSize = canvas.width / gameState.width;

    // Clear the canvas
    context.clearRect(0, 0, canvas.width, canvas.height);

    // Draw the grid
    drawGrid();

    // Draw walls
    gameState.walls.forEach(wall => {
        drawCell(wall.x, wall.y, 'black');
    });

    // Draw fruits
    gameState.fruits.forEach(fruit => {
        drawCell(fruit.x, fruit.y, fruit.color);
    });

    // Draw snakes
    gameState.snakes.forEach(snake => {
        // Darken the snake color for the head by 20%
        const darkerColor = darkenColor(snake.color, 20);

        snake.body.forEach((segment, index) => {
            // Use the darker color for the first segment (the head)
            const colorToUse = index === 0 ? darkerColor : snake.color;
            drawCell(segment.x, segment.y, colorToUse);
        });
    });

}

function darkenColor(hex, percent) {
    // Strip the leading # if present
    hex = hex.replace(/^#/, '');

    // Parse the color into RGB components
    let r = parseInt(hex.substring(0, 2), 16);
    let g = parseInt(hex.substring(2, 4), 16);
    let b = parseInt(hex.substring(4, 6), 16);

    // Decrease each component by the percentage
    r = Math.floor(r * (1 - percent / 100));
    g = Math.floor(g * (1 - percent / 100));
    b = Math.floor(b * (1 - percent / 100));

    // Recompose the color and return it as a hex value
    return '#' + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1).toUpperCase();
}

// Helper function to draw a single cell
function drawCell(x, y, color) {
    context.fillStyle = color;
    context.fillRect(x * cellSize, y * cellSize, cellSize, cellSize);
}

// Function to draw the grid lines (optional)
function drawGrid() {
    context.strokeStyle = '#e0e0e0';
    context.beginPath();
    for (let x = 0; x <= canvas.width; x += cellSize) {
        context.moveTo(x, 0);
        context.lineTo(x, canvas.height);
    }
    for (let y = 0; y <= canvas.height; y += cellSize) {
        context.moveTo(0, y);
        context.lineTo(canvas.width, y);
    }
    context.stroke();
}

const startGameBtn = document.getElementById('startGameBtn');
const resetGameBtn = document.getElementById('resetGameBtn');

connection.on("GameStarted", function () {
    console.log("Game started");
});

connection.on("GameReset", function () {
    console.log("Game reset");
});




connection.on("PlaySound", function (soundFile) {
    const audio = new Audio(`/Sounds/${soundFile}`);
    audio.play().catch(error => console.error("Error playing sound:", error, " soundFile: ", soundFile));
});

startGameBtn.addEventListener('click', function () {
    connection.invoke("StartGame", currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
});

resetGameBtn.addEventListener('click', function () {
    connection.invoke("ResetGame", selectedLevel, currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
});

function changeInstance(instance) {
    currentInstance = instance
    connection.invoke("ChangeInstance", currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}

let timerInterval;
let remainingTime = 120; // 2 minutes in seconds

// Function to start the countdown timer when the game starts
function startTimer() {
    clearInterval(timerInterval); // Ensure no previous timer is running
    remainingTime = 120; // Reset to 2 minutes (120 seconds)
    updateTimerDisplay(); // Update the initial display

    // Start the countdown interval (runs every second)
    timerInterval = setInterval(function () {
        remainingTime--;
        updateTimerDisplay(); // Update the displayed time

        if (remainingTime <= 0) {
            clearInterval(timerInterval); // Stop the timer
            sendRestartGameCommand(); // Send restart game command to the server
        }
    }, 1000); // 1000 ms = 1 second
}

// Function to update the displayed time on the webpage
function updateTimerDisplay() {
    const minutes = Math.floor(remainingTime / 60);
    const seconds = remainingTime % 60;
    const formattedTime = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    document.getElementById('timer').textContent = formattedTime;
}

// Function to send a restart game command to the server when the timer runs out
function sendRestartGameCommand() {
    connection.invoke("RestartGame", currentInstance).catch(function (err) {
        return console.error(err.toString());
    });
}

// Modify the "Start Game" button to trigger the timer
startGameBtn.addEventListener('click', function () {
    connection.invoke("StartGame", currentInstance).then(() => {
        startTimer(); // Start the countdown timer when the game starts
    }).catch(function (err) {
        return console.error(err.toString());
    });
});


