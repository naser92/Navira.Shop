/**
 * @typedef {Object} MenuItem
 * @property {number} permissionId - Permission ID for the menu item
 * @property {string} title - Display title of the menu item
 * @property {string} route - Navigation route, "#" for parent items without navigation
 * @property {number} sortOrder - Sort order for the menu item
 * @property {Array.<MenuItem>} [childs] - Child menu items (optional)
 * @property {number} [id] - Unique ID of the menu item
 */

/**
 * @typedef {Object} UserAccessInfo
 * @property {Array.<MenuItem>} menus - List of menu items for the user
 */

/**
 * @typedef {Object} ApiResponse
 * @property {UserAccessInfo} data - Response data containing user access info
 * @property {boolean} error - Whether there was an error
 */
