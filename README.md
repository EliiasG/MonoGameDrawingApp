# MonoGameDrawingApp
Trying to create a drawing app for the 3rd (or maybe even 4th) time, now in MonoGame
# Shortcuts
| Combination | Function |
| --- | :-: |
| [Ctrl + Shift + G] | Set grid resolution.
| [Ctrl + Shift + W] | Toggle wireframe mode.
| [Ctrl + Shift + C] | Change background color.
| [Ctrl + Z] | Undo. |
| [Ctrl + Y] | Redo. |
| [G] | Toggle grid view. | (only visual, to actually disable the grid set resolution to 0)
| [F] | Focus on selected item. |
# Editing
 - Drag points to move them.
 - Hold [Shift] and drag origin to move origin.
 - Hold [Ctrl] and click to add points at the end of the list.
 - Hold [Alt] and click points to remove them.
# Modifiers
 - Modifiers are operations that are executed every time the geometry is changed, that change the output geometry.  
 - An item may have multiple modifiers.  
 - Modifiers can be applied to be executed on the actual geometry.  
 - Some modifiers may change the structure when applied, since each item can only have one shape an color.  
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
