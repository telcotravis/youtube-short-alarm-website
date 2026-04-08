# Orchestration Log: Website Guide Page

**Date:** 2025-07-15
**Slug:** guide-page
**Requirement:** Add a user-friendly guide page to the website explaining how the app works, including the per-alarm override system.

---

## Phase 1: Repository & Requirement Analysis — Self

**Status:** COMPLETE

### Findings
- Website: ASP.NET Core 9 Razor Pages, Bootstrap 5.3.3, dark theme
- Existing pages: Index, Privacy, Error, Admin/Waitlist
- Navigation: Features, Screenshots, FAQ, Privacy
- Design system: `.section-title`, `.feature-card`, `.text-accent`, `.text-muted-brand`, accordion for collapsible sections
- CSS: warm dark palette (--color-seed: #FF6B35, --color-surface: #1A120D)
- Page model pattern: simple `PageModel` with empty `OnGet()` for static pages

### App Features to Document
- Getting started: API key setup, channel configuration
- Alarm creation: time, days, enable/disable
- Global settings: channel, max duration, volume, replays, videos per alarm, vibration, rising volume
- Per-alarm overrides: "Default" checkbox pattern — checked = use global (-1), unchecked = custom value
- Pro vs Free: 1 alarm free, Pro unlocks unlimited + all overrides + rising volume + playlist + import/export
- Reliability: lock screen, reboot survival, DND bypass, offline fallback
- Permissions explained

### Acceptance Criteria
- Guide.cshtml and Guide.cshtml.cs created following existing patterns
- Navigation link added to _Layout.cshtml
- Content covers: getting started, alarms, settings, overrides, pro features, permissions, troubleshooting
- Styling matches existing site (dark theme, Bootstrap 5.3.3, custom CSS classes)

---

## Phase 2: Implementation — dotnet-developer

**Status:** COMPLETE

- Created `Pages/Guide.cshtml` — 7-section accordion guide covering the full app
- Created `Pages/Guide.cshtml.cs` — simple PageModel
- Modified `Pages/Shared/_Layout.cshtml` — added Guide nav link before Privacy
- No new CSS added — uses existing classes exclusively
- 0 compile errors

---

## Phase 3: Verification — Self

**Status:** COMPLETE

- `get_errors` returned 0 errors across the project
- Guide page uses correct design patterns: container max-width 800px, accordion, text-accent, text-muted-brand, feature-card, Pro badges
- Navigation link correctly placed between FAQ and Privacy
- All 7 sections present: Getting Started, Alarms, Settings, Overrides, Pro vs Free, Permissions, Troubleshooting

---

## Final Summary

| Specialist | Task | Status |
|---|---|---|
| Self (orchestrator) | Repository & requirement analysis | ✅ |
| dotnet-developer | Guide page implementation | ✅ |
| Self (orchestrator) | Verification | ✅ |

### Files Changed
- `Pages/Guide.cshtml` — NEW
- `Pages/Guide.cshtml.cs` — NEW
- `Pages/Shared/_Layout.cshtml` — MODIFIED (nav link added)

### Skipped Phases (with justification)
- Architecture: Simple static page, no structural decisions
- UI/UX design: Following existing page patterns (Privacy page structure)
- Performance: Static content page, no performance concerns
- Testing: Static Razor page with no logic
- Code review: Content page following established patterns

---
