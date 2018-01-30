# GraphQL_DOTNET_Base_App

GraphQL Dot Net basic example -  Console App

# Query
```
query {
   authors (name: ""Nicholas Cage"") {
      id
      name
      country
      books{
         name
         id
         genres {
                 name
                 id
    }}}}
```
# Covered
```
Object
List
Resolve
Params
```

# Packages
```
GraphQL version="0.17.3"
GraphQL-Parser version="2.0.0"
Newtonsoft.Json version="9.0.1"
```

# Framework
```
TargetFramework="net45"
```
