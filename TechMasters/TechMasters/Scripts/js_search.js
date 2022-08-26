/*
*
*	By Pedro Alves
*
*/

$("input#search").on("input", function (){
	var searchTerm = $(this).val();
	searchTerm = searchTerm.toUpperCase().trim();

	var $products = $('.product-item');

	$($products).each(
		function(index, productElem) {
			var name = $(productElem).find('.product-name').text();

			name = name.toUpperCase();

			if (name.indexOf(searchTerm) < 0) {
				$(productElem).hide();
			} else {
				$(productElem).show();
			}
		}
	);
});