function test(a,b) {

    var d = foo(c);
    return a + b + d;

    // this is after a return statement -- should get rid of it
    alert("after return");

    // the var should remain, but the assignment should not
    var c = a - bar(b);

    // the function declaration should remain
    function foo(p)
    {
        return "FOO" + p;
    }

    // this function declaration's only reference gets removed --
    // we SHOULD remove it as unreferenced
    function bar(f)
    {
        return f/10;
    }

    // sbhould get removed
    alert("never get here");
}
