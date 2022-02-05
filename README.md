# Cleaning Clean Architecture - PackIT Edition

The purpose of this repository is to investigate "Clean Architecture" and see if it can be improved upon. The goal is to remove complexity without reducing functionality.

To order to see the transformation incrementally, a branch has been created for each step. Simply compare the branch with the one before it to see the progression.


## Round 0 - Base State Validation

To being with we need to see if the application can be compiled and run. Likewise, we need to verify that all of the automated tests run.

There are only a handful of tests, so this was fairly easy. Just needed to change the connection string to a unique username and give is a password. Which is acceptable for a demonstration project.

Manually testing the end points, we find that the situation isn't great.

The `/api/PackingLists/{id}` endpoint doesn't work at all. Instead of `[HttpGet("{Id}")]`, the route is defined as `[HttpGet("{id:guid}")]`. Capitalization matters here. When it is incorrect, the data binder can't correctly map the values in the route to the function parameters.

Most of the other routes are also bad. Consider this example,

```
[HttpPut("{packingListId:guid}/items/{name}/pack")]
public async Task<IActionResult> Put([FromBody] PackItem command)
```

Since the `command` parameter is marked as coming from the body, the `packingListId` and `name` values are going to be semi-ignored. The client has to provide them or the routing will fail and they will get a 404. But the `Put` method will never actually see the values and instead has to read them from the requests's body, which the client had to also populate.

For now we are only going to fix the Get method.




# PackIT
PackIT is simple "packing list app" built on top of clean architecture and CQRS.
