#TINA-ORM

TINA-ORM (This Is Not An ORM) is a NoSQL-ish interface to Microsoft SQL server inspired by RavenDB.

Tina works with POCOs (Plain Old CLR Objects). That means no custom attributes, no enforced conventions and zero ceremony. Batteries and a coupon for a free hug included.

#Cool Stuff Tina Can Do


##Store stuff
```C#

    MyAwesomeClass myAwesomeVariable = new MyAwesomeClass();
    DoStuff(myAwesomeVariable);

    // Initialize a new instance of Tina
    string connectionString = "Data Source=..."
    Tina tina = new Tina(connectionString);
    
    // Store that variable, just like that.
    tina.Store(myAwesomeVariable);
```

##Query stuff
```C#

    string connectionString = "Data Source=..."
    Tina tina = new Tina(connectionString);
    
    // LINQ - all the cool kids do it
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Order == "Didelphimorphia" // That's Latin for possum
                         select awesomeInstance;
```

##Update stuff
```C#

    string connectionString = "Data Source=..."
    Tina tina = new Tina(connectionString);
    
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Species == "Didelphimorphia" // That's Latin for possum
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
    Tina tina = new Tina(connectionString);
    
    var awesomePossums = from awesomeInstance in tina.Query<MyAwesomeClass>
                         where awesomeInstance.Species == "Didelphimorphia" // That's Latin for possum
                         select awesomeInstance;

    foreach (var possum in awesomePossums)
    {
        // We're switching to platypodes for standard compliance reasons...
        // By the by, platypodes, not platipi, is the plural of platypus. 
        // Tina - The only Open Source project that uses the correct Greek plural of platypus©
        tina.Delete(possum);
    }
```

##Love you uncoditionaly
We <3 our users.
