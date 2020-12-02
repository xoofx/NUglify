var test = {};
test.subNameSpace = {
	 failureTesting: () => {
		  var sub = (info = false) => {
			  console.log("ok " + info);
		  }; sub(true);
	 }
}

window.onload = function() {
	 test.subNameSpace.failureTesting();
}