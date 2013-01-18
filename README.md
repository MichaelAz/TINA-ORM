#TINA-ORM

TINA-ORM (This Is Not An ORM) is a NoSQL-ish interface to SQL databases (Including Microsoft SQL Server and MySQL) inspired by RavenDB. [Get it on NuGet](https://nuget.org/packages/TINA-ORM).

Tina works with POCOs (Plain Old CLR Objects). That means no custom attributes, no enforced conventions and zero ceremony. Batteries and a coupon for a free hug included.

#Cool Stuff Tina Can Do


##Store stuff
```C#

    MyAwesomeClass myAwesomeVariable = new MyAwesomeClass();
    DoStuff(myAwesomeVariable);

    // Initialize a new instance of Tina
    string connectionString = "Data Source=..."

    // Tina also supports the more traditional new statement. Check the wiki for more info.
    Tina tina = Tina.ConnectsTo<MsSql>(connectionString);     

    // Store that variable, just like that.
    tina.Store(myAwesomeVariable);
```

##Query stuff
```C#

    string connectionString = "Data Source=..."
    Tina tina = Tina.ConnectsTo<MsSql>(connectionString);
    
    // LINQ - all the cool kids do it
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Order == "Didelphimorphia" // That's Latin for possum
                         select awesomeInstance;
```

##Update stuff
```C#

    string connectionString = "Data Source=..."
    Tina tina = Tina.ConnectsTo<MsSql>(connectionString);     
    
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Order == "Didelphimorphia"
                         select awesomeInstance;

    foreach (var possum in awesomePossums)
    {
        possum.Species = "Marmosa constantiae"; //White-bellied woolly mouse opossum, the king of Opossums 
    }

    // Updating your data - even your grandma could do it!
    tina.SaveChanges();
```

##Delete stuff
```C#

    string connectionString = "Data Source=..."
    Tina tina = Tina.ConnectsTo<MsSql>(connectionString);
    
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Order == "Didelphimorphia"
                         select awesomeInstance;

    foreach (var possum in awesomePossums)
    {
        // We're switching to platypodes for standard compliance reasons...
        // By the by, platypodes, not platipi, is the plural of platypus. 
        // Tina - The only Open Source project that uses the correct Greek plural of platypus©
        tina.Delete(possum);
    }
```

##And more!
Tina supports other databases (with great extensibility options), custom serializers  and other cool stuff! Check the wiki for more cool stuff!

##Upcoming Features
Upcoming features include support for PostgreSQL, Oracle and DB2, support for more serialization formats (YAML, BSON, MessagePack and more!) and an asynchronus API.