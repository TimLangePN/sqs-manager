# SOM Postman Collections

Here you will find all the steps to add Postman Collections to Bitbucket.

You need to point to the master branch
```
git checkout master 
```
After pointing to the master branch, you will need to pull all the updates that occured
```
git pull
```
Now, you need to add or modify the files that you want to push to bitbucket

After you're adding/modifying the files, you need to promote pending changes from the working directory to the staging area
```
git add .
```
After adding the change to the staging area, you need to move the files to the local repository
```
git commit -m '<Your message here>'
```
Now you can upload the local repository content to the remote repository
```
git push
```