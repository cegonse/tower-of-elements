#!/bin/bash

# Carpeta del codigo en el Git
GITDIR="tu-path-al-git"

# Carpeta del codigo en Unity
UNITYDIR="tu-path-a-assets-source"

cd $GITDIR
git pull
cp -rf $GITDIR/client/ $UNITYDIR/