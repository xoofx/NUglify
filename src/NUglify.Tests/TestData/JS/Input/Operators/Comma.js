function uxp_toggleElementVisibility_return(e)
{
  var o = e.style; 
  return 'none' === o.display 
    ? (o.display = '', 1) 
    : (o.display = 'none', 0); 
}

// we need to keep those parens around the comma operator
// so that we continue to have two arguments, the second of
// which is a comma-operator expression.
var t = foo(bar, (ack,gag));

// the comma-operators in this var statement should be wrapped in parens to
// distinguish them from the var comma separators
var y = (t, foo, bar);

// multiple commas, for op-prec sake
// because they are evaluated left-to-right, there should be no parens in this list
z = a, b, c, document;
