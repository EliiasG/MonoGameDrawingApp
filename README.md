# MonoGameDrawingApp
An app made to draw vector graphics as triangles.  
Using [an external library](https://github.com/NMO13/earclipper) for triangulation, everything in the [EarClipperLib](https://github.com/EliiasG/MonoGameDrawingApp/tree/main/EarClipperLib) folder is from that library.  
# Shortcuts
| Combination | Function |
| --- | --- |
| [Ctrl + Shift + G] | Set grid resolution.
| [Ctrl + Shift + W] | Toggle wireframe mode.
| [Ctrl + Shift + C] | Change background color.
| [Ctrl + Z] | Undo. |
| [Ctrl + Y] | Redo. |
| [G] | Toggle grid view. (only visual, to actually disable the grid set resolution to 0) |
| [F] | Focus on selected item. |
| [R] | Redraw all. (sometimes nessecary when dealing with modifiers that refernce other objects) |
# Editing
 Drag points to move them.  
 Hold [Shift] and drag origin to move origin.  
 Hold [Ctrl] and click to add points at the end of the list.  
 Hold [Alt] and click points to remove them.  
 Drag a point from a line to create a new point.
# Modifiers
 Modifiers are operations that are executed every time the geometry is changed, that change the output geometry.  
 An item may have multiple modifiers.  
 Modifiers can be applied to be executed on the actual geometry.  
 Some modifiers may change the structure when applied, since each item can only have one shape and color.  
 #### WARNING: if a modifier has an item as a parameter, setting the item to the item containing the modifier or the root may cause weird behaviour.  
### A list of the modifiers:
| Name | Function |
| --- | --- |
| Randomize | Moves all points by a random amount constrained by the input. |
| Round | Smooths corners of geometry by creating more points. |
| SimpleMirror | Mirrors the geometry on a line defined by the point at the selected index and the point after that. |
| Mirror | Mirrors the geometry around a selected line, that goes from the origin of the selected item to the first point of the selected item. |
| Subdivide | Turns every line into to points that are 1/4 and 3/4 along the original line, and repeats that a specified amount of times. |
| Flip | Flips the geometry on the selected axes. |
| Scale | Scales the geometry by the selected amount. |
| Rotate | Rotates the gemoetry by the selected amount. |
| Expand | Moves all points away from their original position, also works with negative values to move points closer. |
| Outline | Adds an outline with the selected width and color. |
| Copy | Immitates the geometry of another item. |
# Exporting
An export profile tells the program how to export a file.  
There can be multiple export profiles, and if so, each file will be exported as multiple files.  
To export a project it must contain a Profiles.json file and a Source directory (case sensitive).  
The Profiles.json file is a list of lists, each inner list being a profile.  
The first item in an export profile is the name of the exporter to use (case sensitive).  
The second item in an export profile is the suffix of the exported file, it can begin with a '/' to export to a subfolder with the same name as the input file (so a postfix of *"/Big"* in a Png profile will turn *"SomeSprite.vecspr"* into *"SomeSprite/SomeSpriteBig.png"*).  
The remaning items of the list are parameters for the profile.  
### Types of profiles:
 - "Png": exports to a .png file.
   - The first parameter is an unsigned number, representing the amount of pixels per unit. 
   - The second parameter is a boolean that determines if it should round to whole units (if true, the width and height will always be divisible by the first parameter).
 - "Tris": exports to a .tris file.
   - Has no parameters.  
### Example of a Profiles.json file:
```json
[
    [
        "Png",
        "/Big",
        512,
        true
    ],
    [
        "Png",
        "/Small",
        64,
        true
    ],
    [
        "Tris",
        "/"
    ]
]
```
# Starting a project
To start a project click "Import / Create Project" and select a folder (or create a folder and select it).  
There is [a project](https://github.com/EliiasG/MonoGameDrawingApp/tree/main/ExampleProject) included in the repo.  
### When creating a new project:  
In the project make a Source folder and a Profiles.json file.  
All Vector Sprites must be in the Source folder (or any folder in it) to be exported.  
Vector Sprites must be created using the "Add File" button, just creating an empty file with the .vecspr extention will **NOT** work.


# The .tris format
.tris is a format I invented to store vector graphics as vertices and triangles.  
The .tris file starts with a vertex segment with a color.  
After the vertex segment there is a list of (signed) int32s representing indices, every 3 indices is a triangle.  
### A vertex segment works as follows:  
- The first 4 bytes of the vertex segment represent the color of the vertex as ARGB, if it changes color from the last vertex. 
- The next 8 bytes of the vertex segment represent the position of the vertex (x as a float and y as a float).  
- The next byte will be 1 if the color changes on the next vertex, 0 if it dosent, and 2 if its the end of vertices.
- After each vertex segment there is another vertex segment, unless the last byte was 2.
