# Flowstate.Data.UnityOfWork

Flowstate.Data.UnityOfWork is a small C# library that provides a simple abstraction to manage unity of work instances from the application layer components, while  infrastructure data components have full access to your data persistence strategy details. A default implementation based on [Dapper](https://github.com/DapperLib/Dapper) is built-in, exposing raw connection and transaction objects for your repositories.

## Usage

### ASP.NET

The underlying connection as well as the unity of work should be added on service collection with life time scoped, which is automatically managed by ASP.NET, so that the same unity of work instance will be shared by application and infrastructure data components. 

``` 
TODO...
```

### Simpler console applications

Note that for simpler console applications there isn't automatic management of dependency injection scopes -- like we have in ASP.NET which are delimited by web requests -- and it's your responsibility to define and manage them. The default top-level scope might be enough for a CLI with very straightforward funcionality but not for cron job where you might need to manage scope per processed item.

``` 
TODO...
```
