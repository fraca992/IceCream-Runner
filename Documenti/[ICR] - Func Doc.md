# Functional Documentation

## Entity Tree

|
|- Player Character
|- Ice Cream
|- StreetPiece
|   |- Cell
|- Item:
    |- Obstacle
    |- Power-up

\- Testing Framework

## Functional Description of Entities

### <span style="color:teal">Player Character</span>

#### Properties

* Movement along `X` axis reversible (e.g. an `obstacle` could invert the movement) ---> `Player Properties`
* PC needs to expose `Item` effects' it is subjected to to the outside ---> `Player Properties`

#### Functions

- At the start of a game, the PC is spawned at the `(0,0,0)` position, above the `Street`, with a 1 ball `Ice Cream` ---> `Level Manager`
- PC needs to be able to detect and apply the effects of active `Items` ---> `Player Manager`
- PC needs to move along the `X` axis ---> `Player Manager`
- PC could be able to move vertically along the `Y` axis after a `GameOver` (e.g. ragdoll) or interacting with `Items` ---> `Player Manager`
- PC should not move along the `Z` axis, save for when in a `GameOver` state (e.g. ragdoll) -> `Player Manager`

---

### <span style="color:teal">Ice Cream</span>

#### Properties

- Temperature: it's the health indicator of an IC ball ---> `IceCream Properties`
- IC Ball number: the number of Ice Cream balls on the `Ice Cream`. Once melted, an IC Ball can't be restored by low temperatures, and once all IC Balls are melted it's `GameOver`. ---> `IceCream Properties`
- Heat Modifier: represents the effectiveness of `heat` and `chill` on the IC. It starts at 50%. at 0% heat has no effect, and at 100% chill has no effect. ---> `IceCream Properties`

#### Functions

- Heat/Chill: the IC Ball(s) need to be able to change temperature of a value X. ---> `IceCream Manager`
- Add/Remove IC Balls. if a new IC Ball is added, the previous ones lower their temperature slowly until they reach the topmost IC Ball (the opposite does NOT happen!) ---> `IceCream Manager`
- If an IC Ball is `melted`, all the ones on top of it fall on the ground ---> `IceCream Manager`

---

### <span style="color:teal">Street Piece</span>

#### Properties

* A SP must contain its `budget` ---> `Street Properties`
* A SP must contain its `cells`' data (points & coordinates) ---> `Cell Properties`, managed by `Street Properties`
* An Index, which shows the place of the SP in the `Street FIFO` ---> `Street Properties`
* Unique `Id`, unchangeable unlike the Index, which identifies each SP ---> `Street Properties`

#### Functions

- When a new game starts, N SPs need to be spanwed ---> `Level Manager`
- When a SP ends up behind the PC and is not visible anymore, it needs to be destroyed *along all the `items` it contains!* ---> `Level Manager`
- There must always be N SPs, so when one is destroyed a new one is spawned ---> `Level Manager`
- the `Street`, made by SPs, must move at a variable speed uniformly ---> `Level Manager`
- The cell need to compute the Cell `Coordinates` each times they're required ---> `Street Properties`

---

### <span style="color:teal">Cell</span>

#### Properties

- `Point` value of the Cell ---> `Cell Properties`
- `Coordinates` of the Cell ---> `Cell Properties`

---

### <span style="color:teal">Item</span>

#### Properties

* Item Effects must be readable from the outside ---> `Item Properties`
* cost of the item in `points`. ---> `Item Properties`
* the `Id` of the `SP` the Item was spawned on, useful for destruction later ---> `Item Properties`

#### Functions

- Whenever a new `Street Piece` is spawned, it must be populated by Items ---> `Item Manager`, controlled by the `Level Manager`
- There must be a point `Budget` , and/or a max number of spawnable Items for each SP ---> `Item Manager`, controlled by the `Level Manager`
- Whenever a SP is destroyed, all Items on it must be destroyed as well ---> `Item Manager`, controlled by the `Level Manager`

---

### <span style="color:teal">Testing Framework</span>

The role of this enity is to allow easy and scaleable testing of the game. to allow this, the TF must be ==easy to remove once the game is finished!==. Ideally, all logic must be only implemented in specific testing scripts with no (or little and well documented) changes to other scripts.

#### Properties

* 

#### Functions

- Modifying velocity of the `level scrolling` ---> TBD
- Starting/stopping `level scrolling` ---> TBD
- Spawning the next `Street Piece` (if no SP is present, one is created under the player) ---> TBD
- Deleting the last (farthest) `Street Piece` ---> TBD
- Triggering the `Item Spawning` algorithm on a specific `Street Piece` ---> TBD
- Starting/stopping authomatic `Item Spawning` algorithm on new SP ---> TBD
- modifying the `Street Budget` of a certain `Street Piece` ---> TBD