

![Icon](Graphics/icon.png)


Utilities
========

This package contains some useful classes that commonly are being used in 
different project types. 

Results
-------

Its a cleaner substitution of tupples where a function 
needs to return the success/failure status of its 
operation along side or without the actual result of the operation 
(in case of success). 

It has been implemented in three levels
 * ```Result```: Only represents the success/failure
 * ```Result<T>```: provides the success/failure status and also holds 
a value of type ```T``` which in case of success would be the operation result
   * It's cast-able to both boolean (success/failure) and ```T```   
 * ```Result<TPrimary,TSecondary>```: same as ```Result<T>```, but it can 
carry two different values.

Factory Base
------------

The class ```FactoryBase<TProduct,TArgument>``` provides an implementation 
of _Factory Design Pattern_. Where ```TProduct``` would be the type of what factory would 
be making. and ```TArgument``` is the type of argument passing to ```Make()``` 
method, and factory should decide which the implementation based on this argument.


for example if you have an ```ICar``` interface, and you have implementations like 
```MohaveCar```, ```CamaroCar```, ```CameryCar``` and ```X3Car```. And you want to have a 
factory that makes a car by it's brand name. So for this case, the TProduct would be 
```ICar``` and TArgument would be ```string```.

Next step you would extend ```FactoryBase``` class and implement it's match methods:

 * implement ```MatchesByType(Type,TArgument)``` if your factory would be able to decide 
only by type and the argument. And it does not need to have the instance to make a decision.
 * implement ```MatchByInstance(TProduct,TArgument)``` if your factory needs an instance of 
the object to be able to decide if it matches the argument or not.

you also would need to satisfy the ```factoryMatching``` argument in base class's 
constructor. This argument determines which of two methods above are you using.


__What about my DI?__

If your implementations are needed to be created by any kind of DI or IOC,
all you have to do is to pass a ```Func<Type,object>``` delegate to constructor. 
this delegate will make the Factory class enable to use your Di resolver to 
instantiate your implementations. 

In the other hand, if you are not using a DI or any alternative for instantiating 
your implementations, Factory class would still work while your implementations have 
a non-parametric constructor.

__How The Factory Finds My Implementations?__

By default, the factory, when instantiated, scans the assembly containing your factory class,
(which you derived from FactoryBase<,>) for all implementations of TProduct.
If your implementations are in any other assembly, you can just call the method 
```ScanAssembly``` passing that assembly to it.

__What There is no valid implementation for given Argument?__

Usually the best practice would be to use a __NULL Implementation__ for such situations.
 FactoryBase would return null if it does not find any implementation. But you can change 
this behavior by overriding the method ```DefaultValue``` and return your 
___Null Implementation___ there.

If you have a class derived from TProduct that is actually a NullObject implementation, 
to Prevent the FactoryBase to use it as a valid implementation, simply just Use 
```NullObject``` attribute on your implementation.


Naming Conventions
------------------

The class ```NamingConvention``` allows to parse and convert an string from  
one convention into another. For standard conventions, it can automatically 
detect the source convention. You can also define and use your own conventions 
by simply providing a new instance of ConventionDescriptor object.

__```NamingConvention``` class's methods:__


 * ```Result<ProcessedName> Parse(string name)```
   * This method trys to identify the naming convention that the given name is confirming with,
   by searching across the builtin standard conventions.
   * Returns a successful result of ```ProcessedName``` object containing the detected convention's 
   descriptor and segmented values of the name, if be able to find one. otherwise returns a failure result.
*  ```Render(string[] segments, ConventionDescriptor convention)``` and ```Render(ProcessedName processedName)``` 
   * Takes a separated segments of a name and the target convention's descriptor, directly or via a 
   ProcessedName object, and assembles the segments together to create a name confirming with target convention.
* ```string Convert(string name, ConventionDescriptor convention)```
   * Takes a source name and target convention. Trys to detect the source name's convention and if found one, 
  then will convert given name to target convention.

__Standard Builtin Conventions:__

 You can find predefined standard conventions in ```ConventionDescriptor.Standard``` class.
 it also provides all standard conventions as an array in ```StandardConventions``` property.

The following code shows an example use-case for naming convention classes:

```c#
var names = new[]
{
    "ahmad-mahmud-kababi", "FAT_SNAKE", "_internalShit",
    "lame_snake", "MrPascal", "stupidCamel"
};

var namingConvention = new NamingConvention();



foreach (var name in names)
{
    var parsed = namingConvention.Parse(name);

    Console.WriteLine("--------------------------");

    if (parsed)
    {
        Console.WriteLine("Parsed " + name + ", Detected: " + parsed.Value.Convention.Name);

        Console.WriteLine("-----------");

        foreach (var convention in ConventionDescriptor.Standard.StandardConventions)
        {
            Console.WriteLine(convention.Name + ": " +
                              namingConvention.Render(parsed.Value.Segments, convention));
        }
    }
}
```


Compressions
------------

There are two small methods that simply just wrap up System.IO.Compression tools in two 
extension methods: ```string.CompressAsync(string,Compression,[CompressionLevel])``` and
```string.DeCompressAsync(string,Compression)```. The Compress method, takes and string 
and returns the compressed data as base64 string. And the DeCompress method does exactly 
the opposite.



Plugins
--------

Dotnet application can have plugins by loading other assembly files in the application and 
using reflection to create instances from types.

in this package there is a class named ```PluginManager```, which is a singleton, and will 
provide such functionality easily. For this, first plugin manager needs to find the location to 
your compiled additional code's binaries. Then it need to know which one is the main assembly
(therefore the others would be dependencies). For minimizing the complexity, Plugin-manager class
 does its task obeying a simple convention:

* A plugin is a collection of binaries inside a directory
* This directory's name must be exactly (including .dll) the same as the dependency file. (case in-sensitive)
* Plugin-manager would look up the directory __Plugins__, to find plugin directories

For example consider you wrote a service that you want to export as a plugin. After building your 
 code, it will give you Your.Service.dll file. Then in your client application, which is using the 
plugin-manager class to get to your service, you would have to create the following directory structure:

```
<dir>   |-Plugins
<dir>   |---------|your.service.dll
<file>  |--------------------------|Your.Service.dll
```

And if your service, depends on for example NewtonSoft.Json.dll and Some.Other.Dependency.dll,
 you will also put all of those files beside your own service file:

```
<dir>   |-Plugins
<dir>   |---------|your.service.dll
<file>  |--------------------------|Your.Service.dll
<file>  |--------------------------|NewtonSoft.Json.dll
<file>  |--------------------------|Some.Other.Dependency.dll
```



