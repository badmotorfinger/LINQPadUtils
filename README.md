LINQPadUtils
============

A small collection of extension methods which I find useful for LINQPad on occasion. They display a little more information than the `Dump()` counterpart. Use with caution on large data sets.

[Download the compiled binary here](https://github.com/vincpa/LINQPadUtils/releases)

#### `DumpReflect()`

In addition to the standard `Dump()` output, this will also obtain values by calling methods which do not contain any parameters. Private, internal, protected and private members are also displayed. There's also an option to specify the reflected depth which works a little differently. The main difference being that primitive types are also reflected rather than simply being displayed.

All of the standard overloads of the built-in `Dump()` method are supported.

![DumpReflect Screenshot](https://github.com/vincpa/LINQPadUtils/raw/master/DumpReflectScreenie.jpg)

#### `DumpJson()`

Displays an object graph much like the `Dump()` method, but Json formatted.

![DumpJson Screenshot](https://github.com/vincpa/LINQPadUtils/raw/master/DumpJsonScreenie.jpg)

#### `Reflect()`

Reflects over a type and gets the values required for `DumpReflect()`. Can be used in conjunction with `Dump()`. For example, `obj.Reflect().DumpJson()` or `obj.Reflect().Dump()` which is equivalent to `DumpReflect()`.

### Known issues

For some types `DumpReflect().DumpJson()` will fail due to cyclic dependencies.
