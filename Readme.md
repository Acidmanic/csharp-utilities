

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

__What There is no valid implementation for given Argument?_

Usually the best practice would be to use a __NULL Implementation__ for such situations.
 FactoryBase would return null if it does not find any implementation. But you can change 
this behavior by overriding the method ```DefaultValue``` and return your 
___Null Implementation___ there.

If you have a class derived from TProduct that is actually a NullObject implementation, 
to Prevent the FactoryBase to use it as a valid implementation, simply just Use 
```NullObject``` attribute on your implementation.





