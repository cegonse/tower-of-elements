#!/bin/bash

# Carpeta del codigo en el Git
GITDIR="tu-path-al-git"

# Carpeta del codigo en Unity
UNITYDIR="tu-path-al-unityproject-assets-source-client"

# Mensaje de commit
MSG=$1

cp -rf $UNITYDIR/ $GITDIR/
cd $GITDIR
find . -type f -name '*.meta' -delete
rm -rf ./.vscode
git add .
git commit -m "$MSG"
git push origin master