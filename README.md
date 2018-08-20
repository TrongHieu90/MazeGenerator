# Maze Generator

![Maze1](https://github.com/TrongHieu90/MazeGenerator/blob/master/ImageDocs/MazeGen1.jpg)
![Maze2](https://github.com/TrongHieu90/MazeGenerator/blob/master/ImageDocs/MazeGen2.jpg)

### Translation of p5/javascript code from the [CodingTrain](https://www.youtube.com/user/shiffman) channel into Windows Form with C#

**[Video tutorial](https://www.youtube.com/watch?v=HyK_Q5rrcr4)**

The main algorithm is inside the [Form1.cs](https://github.com/TrongHieu90/PathfindingAStar/blob/master/WindowsFormsApp2/WindowsFormsApp2/Form1.cs) file. There are some modifications to make it work inside Windows Form (which makes you appreciate the p5 library even more with how it simplifies things)

Because we can move diagonally in any direction, the heuristic function is Euclidean distance which yields accepted running time. There are many other heuristic algorithms inside the source to be implemented and further developed.
