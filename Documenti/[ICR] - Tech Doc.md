# Technical Documentation

## Scripts

In this section, I'll present every script in the project and describe the public properties and methods exposed.

### <span style="color:teal">CellProperties</span>

#### Public Properties

```c#
int cellPoints; // Point value of the cell.

Vector3 cellCoordinates; // Coordinates of the Cell
```

### <span style="color:teal">StreetProperties</span>

Contains the properties of the Street Piece. It's a simple script

#### Public Properties

```c#
int streetBudget; // Budget that the Street Piece has. 

int streetIndex; // Index of the Street Piece. it changes from N (farthest) to 0 (closest), like a FIFO Memory.

int streetId; // unique Id (just a sequential number) of that Street Piece.
```

#### Public Methods

```C#
Cell[] GetStreetCells(); // Methods that returns an array of the N cells of the street, with updated coordinates.
```

### <span style="color:teal">LevelManager</span>

#### Public Properties

TBD

#### Public Methods

TBD