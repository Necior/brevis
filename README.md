# brevis

Tiny 3D app for previewing OFF files.

**Note**: The code is ugly, unmaintanable or probably not understandable by anyone other than the author-at-the-time-of-coding.
I was learning the topic od 3D graphics while developing this app and I cut corners _a lot_.
Expect weirdly names variables, code duplication, dead code, useless computations, lack of UI, wrongly implemented algorithms and more.

## Features

* Load simple (not all) OFF files (examples at https://people.mpi-inf.mpg.de/~kettner/proj/obj3d/index.html);
* Move and rotate the object/camera;
* Show object as a wireframe;
* Show object as a mesh:
    - With one color,
    - With random colors,
    - With texture;
* Z-buffer;
* Backface culling;
* Phong reflection model;
* Linearly interpolated fog;
* Transparent triangles.
