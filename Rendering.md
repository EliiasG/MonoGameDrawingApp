# Rendering
### This document contains an explanation of how a library for drawing triangle vector graphics might work.
## Models
Models will be loaded as a list of colored vertices and indices forming triangles. It is important that triangles are drawn in order.  
Models should also be allowed to be quite big, 2^(14 to 16) triangles should be enough.
## Drawing models
Every loaded model should have an (or possibly multiple) array with information (position, rotation, z-index, ect) to draw instances.  
The shader should be able to dertermine a depth value for each instance.  
Up to 2^(16 to 18) total inceances should be enough. (although more would be nice)