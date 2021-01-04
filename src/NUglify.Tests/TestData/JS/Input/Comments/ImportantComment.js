/*!
 * this is an example of an "important" comment
 * that we want to keep
 */
function foo() {/*! this too */}
/* regular comment that will get stripped */
var a = 12;
/*! * this is another one, but it's between a couple var statements */
var b = 13;

/** @preserve This is an important comment because of the @preserve token */

/**
 ** @license  And so is this, because of the @license token
 */

// make sure an important comment at the end of a block is also kept
for(var ndx = 0; ndx < 10; ++ndx)
{
    /*! inside a block */
}
