#!/bin/bash

# Carpeta del codigo en el Git
GITDIR="/c/Users/Adrian/Documents/ice-game"

# Carpeta del codigo en Unity
UNITYDIR="/c/Users/Adrian/Documents/Unity/IceGame/Assets/source"

# Mensaje de commit
MSG=$1

`cp -rf $UNITYDIR/ $GITDIR/`
`cd $GITDIR`
`find . -type f -name '*.meta' -delete`
`git add .`
`git commit -m $MSG`
`git push origin master`