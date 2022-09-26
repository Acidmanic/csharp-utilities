

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

