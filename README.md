# RssManager

- Considering size of task, was decided not to make any special architecture (clean or n-layer).

- Main news update functionality is located in service folder and implemented via BackgroundService.

In order to test application you have to write several commands in package manager console:

```
Add-migration InitialMigration
Update-database
```

Also you have to sign in (Auth implemented with Auth0), you can use google account for this. Dont type anything in client secret field in Swagger, looks like its a bug that that field is appears.

Besides main tasks there were added some functionality

- Unsubscribe from feed
- Delete all posts
