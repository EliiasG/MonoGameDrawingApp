# Animations
Animations are currently not implemented, this is just a document where i write down my ideas.  
All the ideas this document describes are nothing but ideas, and therefor subject to change.  
The ideas are also not directly tied to this app, and may be implemented somewhere else.
# Features
Here is a list of features the animation system will need, the names of these features are not yet decided.
## Images / graphics
Images are just static images, these will probably be implemented as vector grapics.
## Bodies / assemblies
A body is a tree of nodes and images. (and possibly more things like hitboxes, lights or IK)  
Nodes have a position, scale and rotation, and can have children.  
Images also have a position, scale and rotation, but should not have children.  
All positions, scales and rotations are local to the parent.  
## Skeletons / structures
A skeleton is a loose description of a body, it describes a tree of nodes, but not thier transformations.  
A body can "implement" a sekelton, by assigning some of its nodes to nodes in the skeleton.  
The body should follow the overall structure of the skeleton, but can be more detailed and have more layers.  
Skeletons should be able to be derived from other skeletons, however composition will not be possible as playing an animation would then have to specify "where" to play it.  
## Animations
Animations describe how a body implementing a skeleton should move.  
Animations only have access to nodes described in their skeleton, and should therefore work on any bodies implementing their skeleton, or any skeleton derived from it.  
Animations take any amount of parameters, (most commonly something like "time" or "frame") and can offset scales, positions and rotations of nodes using curves or expressions.