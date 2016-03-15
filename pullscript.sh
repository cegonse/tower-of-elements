#!/bin/bash

# Carpeta del codigo en el Git
GITDIR="/c/Users/Adrian/Documents/ice-game"

# Carpeta del codigo en Unity
UNITYDIR="/c/Users/Adrian/Documents/Unity/IceGame/Assets/source"


`cd $GITDIR`
`git pull`
`cp -rf $GITDIR/client/ $UNITYDIR/`