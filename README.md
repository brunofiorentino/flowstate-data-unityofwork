# Flowstate.Data.UnityOfWork

Flowstate.Data.UnityOfWork is a simple C# library that provides a small set of abstractions to manage unity of work instances from an application layer with default implementations based on Dapper.

## Usage

### ASP.NET

Your application layer components should consume the interfaces in order to delimit transactions while your repositories should consume the concrete classes -- and access connection and transaction related members for the default Dapper based implementations. The underlying connection as well as the unity of work should be added on service collection with life time scoped, which is automatically managed by ASP.NET, so the same instance will be shared by application and infrastructure/data layer components.

``` 
TODO...
```

### Simple Console App

For simpler console applications there isn't automatic management of dependency injection scopes -- like we have in ASP.NET which are delimited by web requests -- and it's your responsibility to define and manage them. The default top-level scope might be enough for a CLI with very straightforward funcionality but now for cron job where you might need to manage scope per proceeded record.

``` 
TODO...
```
