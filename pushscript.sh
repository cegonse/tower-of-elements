#!/bin/bash

# Carpeta del codigo en el Git
GITDIR="tu-path-al-git"

# Carpeta del codigo en Unity
UNITYDIR="tu-path-al-unityproject-assets-source"

# Mensaje de commit
MSG=$1

`cp -rf $UNITYDIR/ $GITDIR/client/`
`cd $GITDIR`
`find . -type f -name '*.meta' -delete`
`git add .`
`git commit -m $MSG`
`git push origin master`