# Changelog

تمام تغییرات مهم پروژه NaviraShop در این فایل ثبت می‌شود.

فرمت این فایل بر اساس مفهوم Keep a Changelog تنظیم شده و پیام‌های Git بهتر است با Conventional Commits هماهنگ باشند.

## [Unreleased]

### Added
- Added the vinext/RSC build adapter required to emit a Sites-compatible deployment artifact.
- Completed the responsive storefront Header with dynamic navigation, accessible submenu, search, account, favorites, cart badge, and mobile drawer interactions.
- Added a data-driven Footer with newsletter UI, grouped navigation, contact/social details, trust badges, and legal links.
- Added typed layout view models and isolated mock storefront layout data for future ASP.NET API replacement.
- Created initial project documentation structure.
- Added `docs/PRD.md` for product requirements.
- Added `docs/ARCHITECTURE.md` for project architecture and folder structure.
- Added `docs/DESIGN_SYSTEM.md` for RTL-first glassmorphism design system.
- Added `docs/API_CONTRACT.md` for backend API communication rules.
- Added `docs/AI_RULES.md` for AI-assisted development standards.
- Implemented `Header` layout primitive (`src/framework/ui/layout/header/Header.tsx`) with brand, glass search field, and auth/cart actions, following the RTL-first glassmorphism design system.
- Integrated `Header` into the root layout (`src/app/layout.tsx`).

### Fixed
- Displayed the literal `NaviraShop` brand text in the Header instead of a translated label, per the branding requirement.
- Wrapped the Header search field in a semantic `<form>` with an accessible label so it behaves correctly as a search landmark.
- Kept the auth action's accessible name (`aria-label="ورود | ثبت‌نام"`) intact on narrow viewports where the visible label text is hidden.

### Planned
- Initialize Next.js 15 project structure.
- Configure TypeScript strict mode.
- Set up SCSS Modules and global style tokens.
- Implement base RTL layout.
- Implement core API infrastructure.
- Set up TanStack Query and Redux Toolkit infrastructure.
- Create base framework UI primitives.

## [0.1.0] - 1405-05-19 / 2026-08-10

### Added
- Defined initial technical direction for NaviraShop.
- Selected Next.js 15 App Router as frontend framework.
- Selected TypeScript for type-safe development.
- Selected SCSS Modules for component-level styling.
- Selected Redux Toolkit for shared client state where needed.
- Selected TanStack Query for server state management.
- Selected Axios for centralized API communication.
- Defined ASP.NET 9 backend integration approach.
- Defined RTL-first and Persian-first UI direction.
- Defined iOS-like glassmorphism visual style for mug e-commerce experience.
