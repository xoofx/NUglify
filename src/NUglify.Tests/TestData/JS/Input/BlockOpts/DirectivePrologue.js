// directive prologues aren't really expressions, even though they are
// technically expression statements consisting of only a single string
// literal. We don't want to consider them expressions, though, because
// we don't want to combine them with subsequent expressions -- they 
// have to be stand-alone.
function one(a,b)
{
    "use strict";
    a = b + b;
    return b + a;
}

function two(c,d)
{
    //! no semi-colons -- they are implicit and should still count
    "some other prologue"
    "use strict"
    c = d + c + d;
    return c;
}

function three(e,f)
{
    "use strict"
    var d = e + f;
    // this isn't a directive prologue, so it should get combined
    // with the return expression. But then when we evaluate literals,
    // it will get stripped out.
    "this is not a prologue directive";
    return d;
}