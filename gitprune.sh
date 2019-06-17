#!/bin/bash
# deletes branches merged to master at the remote (origin)

git fetch --prune
branch=$(git rev-parse --abbrev-ref HEAD)
git branch -r --merged | grep origin | grep -v '>' | grep -v master | grep -v $branch | awk '{split($0,a,"origin/"); print a[2]}' | xargs git push origin --delete
git branch --merged | grep -v master | grep -v $branch | xargs git branch -d
git remote prune origin
