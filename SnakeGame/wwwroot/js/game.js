const canvas = document.getElementById('gameCanvas');
const context = canvas.getContext('2d');

let cellSize = canvas.width / 30; // Adjusted to fit the grid size
let currentInstance = 0;

// Initialize SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .build();

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



function updateLeaderboard(gameState) {
    // Sort the snakes by body length (size), in descending order (biggest snake first)
    const snakes = gameState.snakes;
    const sortedSnakes = snakes.sort((a, b) => b.body.length - a.body.length)

    // Select the leaderboard element
    const leaderboard = document.querySelector('ul');

    // Clear the current leaderboard
    leaderboard.innerHTML = '';

    // Loop through each snake and create a new list item for the leaderboard
    sortedSnakes.forEach((snake, index) => {
        // Create a list item
        const listItem = document.createElement('li');

        // Set the text content to the snake's rank, name, and size
        listItem.textContent = `${index + 1}. ${snake.name}`; // Adding rank (1., 2., etc.)

        // Set the color of the text to match the snake's color
        listItem.style.color = snake.color;

        // Append the list item to the leaderboard
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
        connection.invoke("AddSnake", color, playerName, currentInstance).catch(function (err) {
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
    '#FF3333', // Vibrant Red
    '#33FFD7', // Vibrant Teal
    '#FFC733', // Vibrant Golden Yellow
];

// Function to return a random color from the preset list
function getVibrantRandomColor() {
    const randomIndex = Math.floor(Math.random() * vibrantColors.length); // Random index based on the array length
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

