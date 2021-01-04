//! start my implicit module
function n(n,t){return n+t}export function mul(n,t){return n*t}function t(n){return mul(n,n)}function i(n,t){if(t==0)return 1;for(var i=1;--t;)i=mul(i,n);return i}export const pi=3.1415927,negOne=-1;export var accumulator=0,foo="bar";export{n as sum,t as square,i as pow}//! end my implicit module
