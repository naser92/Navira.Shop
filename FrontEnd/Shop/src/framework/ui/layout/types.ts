export interface NavigationChild { label: string; href: string; }
export interface NavigationItem { label: string; href: string; children?: readonly NavigationChild[]; }
export interface FooterLinkGroup { title: string; links: readonly NavigationChild[]; }
export interface ContactItem { label: string; value: string; href: string; }
export interface SocialLink { label: string; href: string; }
export interface TrustBadge { title: string; description: string; }
export interface HeaderViewModel { navigation: readonly NavigationItem[]; cartItemCount: number; userDisplayName?: string; }
export interface FooterViewModel { description: string; linkGroups: readonly FooterLinkGroup[]; contacts: readonly ContactItem[]; socialLinks: readonly SocialLink[]; trustBadges: readonly TrustBadge[]; }
