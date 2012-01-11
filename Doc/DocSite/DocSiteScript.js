/*
 * DocSite JavaScript and Cookie Utilities
 * written by Dave Sexton
 * http://www.codeplex.com/DocProject
 */
var docSiteSidebarSelectedButton = null, docSiteSidebarVisibleElement = null;

/*
 * The following code is executed when the page loads.
 */
if (document.getElementById)
{
	var handle = document.getElementById("docsite_sidebar_handle");

	if (handle.addEventListener)
	// W3C event model
	{	
		handle.addEventListener("mousedown", docsiteSidebarHandle_mousedown, false);
	}
	else
	// IE event model
	{
		handle.attachEvent("onmousedown", docsiteSidebarHandle_mousedown);
		handle.attachEvent("onmouseup", docsiteSidebarHandle_mouseup);
		handle.attachEvent("onmousemove", docsiteSidebarHandle_mousemove);
	}
	
	// enablePersistSidebarHandle is defined outside of this script based on site settings.
	if (typeof enablePersistSidebarHandle != "undefined" && enablePersistSidebarHandle)
		refreshSidebarHandle();
}

/*
 * Toggles CSS to select the specified sidebar button.
 */
function selectDocsiteSidebarButton(button, contentElement)
{
	if (button != docSiteSidebarSelectedButton)
	{
		if (docSiteSidebarSelectedButton != null)
			docSiteSidebarSelectedButton.className = "sidebar_button";
		
		docSiteSidebarSelectedButton = button;
		button.className = "sidebar_button_selected";
		
		if (docSiteSidebarVisibleElement != null)
			docSiteSidebarVisibleElement.className = "sidebar_content_hidden";
		
		docSiteSidebarVisibleElement = contentElement;
		contentElement.className = "sidebar_content_visible";
	}
}

/*
 * The following functions handle mouse events to control the resizing 
 * of the sidebar.
 */
function docsiteSidebarHandle_mousedown(eventObj)
{
	var handle = document.getElementById("docsite_sidebar_handle");
	
	if (!handle.drag)
	{
		handle.drag = true;
		
		if (handle.setCapture)
			handle.setCapture(true);
		else
		{
			eventObj.stopPropagation();
			window.addEventListener("mouseup", docsiteSidebarHandle_mouseup, true);
			window.addEventListener("mousemove", docsiteSidebarHandle_mousemove, true);
			
			if (navigator.userAgent.indexOf("Gecko") > -1)
			// FireFox 2.0.0.3 does not raise mousemove and mouseup over an IFRAME
			// so the content is hidden while the sidebar is resized
			{
				var content = document.getElementById("docsite_content");
				content.style.visibility = "hidden";
			}
		}
	}
}

function docsiteSidebarHandle_mouseup(eventObj)
{
	var handle = document.getElementById("docsite_sidebar_handle");
	
	if (handle.drag)
	{
		handle.drag = false;
		
		if (handle.releaseCapture)
			handle.releaseCapture();
		else
		{
			eventObj.stopPropagation();
			window.removeEventListener("mouseup", docsiteSidebarHandle_mouseup, true);
			window.removeEventListener("mousemove", docsiteSidebarHandle_mousemove, true);
			
			if (navigator.userAgent.indexOf("Gecko") > -1)
			// see the mousedown event handler for more info
			{
				var content = document.getElementById("docsite_content");
				content.style.visibility = "visible";
			}
		}
		
		// enablePersistSidebarHandle is defined outside of this script based on site settings.
		if (typeof enablePersistSidebarHandle != "undefined" && enablePersistSidebarHandle)
			persistSidebarHandle();
	}
}

function docsiteSidebarHandle_mousemove(eventObj)
{
	var handle = document.getElementById("docsite_sidebar_handle");
		
	if (handle.drag)
	{		
		var sidebar = document.getElementById("docsite_sidebar");
		var content = document.getElementById("docsite_content");

		if (!eventObj)
			eventObj = event;
			
		var handle_width = handle.clientWidth;
		var x = eventObj.clientX - handle_width / 2;

		if (x < 0)
			x = 0;
		else if (x > document.body.clientWidth - handle_width)
			x = document.body.clientWidth - handle_width;
		
		var handleStyle = (handle.runtimeStyle) ? handle.runtimeStyle : handle.style;
		var sidebarStyle = (sidebar.runtimeStyle) ? sidebar.runtimeStyle : sidebar.style;
		var contentStyle = (content.runtimeStyle) ? content.runtimeStyle : content.style;
		
		// FireFox 2.0.0.3 requires "px"
		handleStyle.left = x + "px";
		sidebarStyle.width = x + "px";
		contentStyle.left = (x + handle_width) + "px";
	}
}

/*
 * The following functions handle persistence of the sidebar's current state.
 */
function persistSidebarHandle()
{
	var handle = document.getElementById("docsite_sidebar_handle");
	var handleStyle = (handle.currentStyle) ? handle.currentStyle : handle.style;

	setCookie("sidebar", "position=" + handleStyle.left);
}

function refreshSidebarHandle()
{
	var x = getCookie("sidebar", "position");

	if (x)
	{
		var handle = document.getElementById("docsite_sidebar_handle");
		var sidebar = document.getElementById("docsite_sidebar");
		var content = document.getElementById("docsite_content");
		
		var handleStyle = (handle.runtimeStyle) ? handle.runtimeStyle : handle.style;
		var sidebarStyle = (sidebar.runtimeStyle) ? sidebar.runtimeStyle : sidebar.style;
		var contentStyle = (content.runtimeStyle) ? content.runtimeStyle : content.style;
		
		x = parseInt(x);
		
		// FireFox 2.0.0.3 requires "px"
		handleStyle.left = x + "px";
		sidebarStyle.width = x + "px";
		contentStyle.left = (x + handle.clientWidth) + "px";
	}
}

/*
 * Gets the cookie with the specified name.
 * name:					Name of the cookie to be retrieved.
 * crumb:					[optional] Name of a sub-value to be retrieved.
 *								If this argument is not specified the entire cookie is returned.
 * separator:			[optional] String that separates crumbs in the cookie's value.
 *								The default value is a semicolon ";".
 * 
 * Example: 
 * setCookie("hello", "person=Joe;world=earth");
 * var cookie = getCookie("hello", "world");
 * alert(cookie);		// earth
 */
function getCookie(name, crumb, separator)
{
	var cookies = document.cookie;
	
	if (!cookies)
		return null;
		
	cookies = cookies.split("; ");
	
	for (var c = 0; c < cookies.length; c++)
	{
		var cookie = cookies[c].split("=");
		
		if (name == cookie[0])
		{
			var value = unescape(cookie[1]);
			
			if (!crumb || !value)
				return value;
				
			var crumbs = value.split((separator == null) ? ";" : separator);
			
			for (var s = 0; s < crumbs.length; s++)
			{
				var pair = crumbs[s].split("=");
				
				if (crumb == pair[0])
					return pair[1];
			}
		}
	}
	
	return null;
}

/*
 * Sets the cookie with the specified name to the specified value.
 * name:					Name of the cookie.
 * value:					[optional] Unescaped value of the cookie.
 * expires:				[optional] Boolean that indicates whether the cookie expires
 *								- or - a valid UTC string or Date object for explicit expiration.
 *								The default value is null/false (the cookie never expires).
 * path:					[optional] Uri path information to which the cookie applies.
 * domain:				[optional] Domain name to which the cookie applies.
 * secure:				[optional] Boolean that indicates whether the cookie is used 
 *								over HTTPS only.
 * [returns]			String representation of the cookie.
 */
function setCookie(name, value, expires, path, domain, secure)
{
	var cookie = ((name == null) ? "" : name.toString()) + "=" + escape(value);
	
	if (!expires)		// default
	{
		expires = new Date();
		expires.setTime(expires.getTime() + 3E12);		// a long time
	}
	else if (typeof(expires) != "boolean")
	{
		expires = new Date(expires); // expires can be a Date object or a date string.
	}
	else
		// expires is true - omit the value so the cookie will expire with the session.
		expires = false;
	
	if (expires)
		cookie += "; expires=" + expires.toUTCString();
		
	if (path)
		cookie += "; path=" + path;
		
	if (domain)
		cookie += "; domain=" + domain;
		
	if (secure)
		cookie += "; secure";
	
	// Setting this property will only add or replace the cookie with the specified 
	// name - not all cookies in the document.
	document.cookie = cookie;
	
	return cookie;
}