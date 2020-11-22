jQuery(document).ready(function(){
	jQuery(".menu-button i").click(function(){
		jQuery(".menu").slideToggle();

	});
		$(".sub-more").click(function(){
			$(".sub-more > ul").slideToggle();
		});

	jQuery(window).resize(function(){
		var sreeenSize = jQuery(window).width();
		if (sreeenSize > 768) {
			jQuery(".menu").removeAttr("style");
		} 
			

	});
	



})
