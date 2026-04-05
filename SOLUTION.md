# YT Shorts Alarm — Website Solution Document

**Date:** April 4, 2026  
**Author:** Architect  
**Status:** READY FOR DEVELOPMENT

---

## 0. Resolved Questions

| # | Question | Answer |
|---|----------|--------|
| Q1 | Target domain | `ytshortsalarm.kardsen.com` ✅ Confirmed |
| Q2 | Launch status | **Pre-launch** — "Coming Soon" CTA with email signup |
| Q3 | Tech stack | **ASP.NET Core** (.NET website) |
| Q4 | Email signup / waitlist | Yes — server-side form with database storage |
| Q5 | Google Play Store URL | Not yet available — use placeholder |
| Q6 | App icon / marketing assets | Screenshots provided; icon to be extracted from APK |
| Q7 | Privacy policy source | Pulled verbatim from app's `PrivacyPolicyActivity` (see Appendix A) |
| Q8 | Analytics | TBD — recommend Plausible (privacy-friendly) |
| Q9 | Developer name | "Kardsen" |
| Q10 | Contact email | TBD — `support@kardsen.com` placeholder |
| Q11 | Hosting | **InterServer** shared/VPS hosting |
| Q12 | SEO keywords | "youtube shorts alarm", "wake up youtube", "youtube alarm clock android" |

---

## 1. Project Overview

### Purpose
A single-page promotional website for the **YT Shorts Alarm** Android app that:
1. Showcases the app's features with the provided screenshots
2. Drives downloads from the Google Play Store
3. Hosts the legally-required **Privacy Policy** (needed for Play Store listing)
4. Hosts the **YouTube API Services** compliance pages (ToS links, attribution)
5. Builds pre-launch buzz with an optional email signup

### Target Audience
- Android users who want a more engaging alarm experience
- YouTube Shorts viewers / content creator fans
- People searching for "YouTube alarm clock" or "wake up to YouTube"

---

## 2. Site Architecture

### Pages

| Page | Route | Purpose |
|------|-------|---------|
| **Landing Page** | `/` | Hero, features, screenshots, download CTA, FAQ, email signup |
| **Privacy Policy** | `/Privacy` | Legal requirement for Play Store + YouTube API compliance |
| **Support** | `/#faq` | FAQ section on landing page + contact email in footer |

### Sitemap

```
/                     → Landing page (Razor Page with anchor sections)
  /#hero              → Hero with app name, tagline, "Coming Soon" + email signup
  /#how-it-works      → 3-step explanation
  /#features          → Feature grid/cards
  /#screenshots       → Screenshot carousel/gallery
  /#faq               → Frequently asked questions
  /#download          → Final CTA with email signup
/Privacy              → Privacy Policy (standalone Razor Page)
```

---

## 3. Technology Stack

### ASP.NET Core Razor Pages

| Layer | Technology | Rationale |
|-------|-----------|-----------|
| **Framework** | ASP.NET Core 9.0 (Razor Pages) | .NET requested; Razor Pages ideal for page-focused sites |
| **Styling** | Bootstrap 5.3 + custom CSS with CSS custom properties | Responsive grid, components; custom theme matches app's Material3 palette |
| **Interaction** | Vanilla JS (minimal) | Smooth scroll, screenshot carousel, FAQ accordion, mobile nav |
| **Fonts** | Google Fonts (Inter or Roboto) | Matches Material Design, free |
| **Icons** | Bootstrap Icons or Material Symbols (inline SVG) | Consistent with app's icon set |
| **Database** | SQLite (via EF Core) | Lightweight; stores email signups for pre-launch waitlist |
| **Email** | Optional SMTP integration | Send confirmation emails for waitlist signups |
| **Deployment** | InterServer hosting (Linux or Windows VPS/shared) | Published as self-contained or framework-dependent deployment |
| **Reverse Proxy** | Nginx or IIS (depending on InterServer OS) | Proxies to Kestrel; handles HTTPS via Let's Encrypt |
| **Analytics** | Plausible (optional) | Privacy-friendly, no cookie banner needed |

### Why ASP.NET Core Razor Pages?
- .NET stack as requested
- Razor Pages is simpler than full MVC for a page-focused site (no controllers needed)
- Server-side rendering for SEO — search engines get fully-rendered HTML
- Built-in antiforgery for the email signup form
- EF Core + SQLite for zero-config waitlist database
- Easy to extend later (admin panel, blog, API endpoints)
- Self-contained publish means no .NET runtime needed on server

---

## 4. Design System

### Brand Colors (from app's Material3 theme)

```css
:root {
  /* Core brand */
  --color-seed:            #FF6B35;  /* Sunrise Coral — primary brand color */
  
  /* Light mode */
  --color-primary-light:   #9C4400;
  --color-on-primary-light:#FFFFFF;
  --color-secondary-light: #77574B;
  --color-tertiary-light:  #675E2F;
  --color-surface-light:   #FFF8F5;
  --color-on-surface-light:#231A14;
  
  /* Dark mode */
  --color-primary-dark:    #FFB693;
  --color-on-primary-dark: #562100;
  --color-secondary-dark:  #E7BDB0;
  --color-tertiary-dark:   #D3C78D;
  --color-surface-dark:    #1A120D;
  --color-on-surface-dark: #F1DFD6;
  
  /* Semantic */
  --color-cta:             #FF6B35;  /* Call-to-action buttons */
  --color-cta-hover:       #E55A25;
  --color-success:          #4CAF50;
  --color-text-muted:      #9E8E84;
}
```

### Typography
- **Headings:** Inter (or Roboto) Bold, 700 weight
- **Body:** Inter (or Roboto) Regular, 400 weight
- **Mono (code/tech):** JetBrains Mono (for any technical text)

### Dark Mode
- The website should default to dark mode (matching the app's dark screenshots)
- Support `prefers-color-scheme` media query for automatic light/dark switching
- All screenshots are dark-themed, so dark-first design is more cohesive

### Responsive Breakpoints
| Breakpoint | Target |
|-----------|--------|
| `< 640px` | Mobile (single column) |
| `640px–1024px` | Tablet (2-column features) |
| `> 1024px` | Desktop (3-column features, side-by-side layout) |

---

## 5. Page Specifications

### 5a. Landing Page (`/`)

#### Section 1: Hero
- **Layout:** Split — left text, right phone mockup
- **Content:**
  - App icon (top-left, 64px)
  - H1: "Wake up to your favorite YouTube Shorts"
  - Subtitle: "YT Shorts Alarm plays the latest Short from any YouTube channel as your alarm. No more boring beeps."
  - CTA button: "Coming Soon" badge + email signup form
  - Secondary CTA: "Learn more ↓" scroll link
- **Visual:** Phone mockup showing the alarm player screen (screenshot 7 — the one with "Rise and Shine You Magnificent")
- **Background:** Dark gradient matching app's surface color
- **Email Signup Form:** Simple inline form — email input + "Notify Me" button, server-side POST to `/` handler

#### Section 2: How It Works
- **Layout:** 3-step horizontal cards
- **Content:**
  1. **Pick a channel** — "Enter any YouTube channel handle, URL, or ID"
  2. **Set your alarm** — "Choose your time, days, and playback preferences"
  3. **Wake up inspired** — "The latest Short plays full-screen, even over your lock screen"
- **Icons:** Material Symbols (play_circle, alarm, smartphone)

#### Section 3: Features Grid
- **Layout:** 2×3 or 3×2 grid of feature cards
- **Cards:**
  1. **Smart Video Selection** — "Automatically finds the newest embeddable Short from your chosen channel"
  2. **Per-Alarm Customization** — "Different channel, volume, replays, and duration for each alarm"
  3. **Reliable Delivery** — "Uses Android's exact alarm API — survives Doze mode and reboots"
  4. **Rising Volume** — "Gentle wake-up: volume ramps from whisper to full over 30 seconds"
  5. **Playlist Mode** — "Play multiple Shorts in sequence — skip blocked videos automatically"
  6. **Privacy First** — "No accounts. No tracking. No ads. All data stays on your device."
- **Each card:** Icon + title + 1-2 sentence description

#### Section 4: Screenshots
- **Layout:** Horizontal scrollable carousel (mobile) / grid (desktop)
- **Screenshots:** All 7 provided images displayed in phone mockup frames
- **Labels under each:**
  1. "Alarm dashboard"
  2. "Channel selection"
  3. "Alarm history"
  4. "Edit alarm"
  5. "Playback overrides"
  6. "Settings & permissions"
  7. "Alarm playing"
- **Interaction:** Click/tap to enlarge (lightbox)

#### Section 5: Technical Highlights
- **Layout:** Compact list or badge-style
- **Items:**
  - Material Design 3 UI
  - Android 10+ (API 29+)
  - Encrypted local storage
  - YouTube Data API v3
  - Export/import configuration
  - 48+ unit tests
  - Zero third-party tracking SDKs

#### Section 6: FAQ
- **Layout:** Accordion (expand/collapse)
- **Questions:**
  1. "Is YT Shorts Alarm free?" → "Yes, completely free. No ads, no subscriptions, no in-app purchases."
  2. "Do I need a YouTube API key?" → "The app uses the YouTube Data API to find videos. You'll need to provide your own API key (free from Google Cloud Console). Setup takes about 2 minutes."
  3. "What happens if I have no internet when the alarm fires?" → "The app falls back to your device's default alarm tone within 15 seconds. Your alarm always goes off."
  4. "Does it work with any YouTube channel?" → "Yes — enter a @handle, channel URL, or channel ID. The app finds the latest embeddable Short-style video."
  5. "Will my alarm go off after a reboot?" → "Yes. All alarms are rescheduled automatically after your device restarts."
  6. "Does the app drain my battery?" → "No. The app does nothing between alarms. It uses Android's native AlarmManager which is battery-optimized."
  7. "Why does the app need so many permissions?" → "Each permission is essential for alarm reliability: exact timing, waking the screen, surviving battery optimization, and showing notifications."

#### Section 7: Final CTA
- **Layout:** Centered, full-width dark section
- **Content:**
  - H2: "Ready to wake up differently?"
  - Email signup form (same as hero — "Notify Me" for pre-launch)
  - "Free. No ads. No tracking."
  - When app launches: swap signup for Google Play badge

#### Footer
- App name + version
- Developer: "Made by Kardsen"
- Links: Privacy Policy | GitHub (if open source) | Contact
- YouTube API attribution: "This app uses YouTube API Services. By using this app you agree to be bound by the YouTube Terms of Service."
- Links: YouTube Terms of Service | Google Privacy Policy

### 5b. Privacy Policy Page (`/Privacy`)

- **Content:** Verbatim from the app's `PrivacyPolicyActivity` (see Appendix A for full text)
- **Additions for web:**
  - Last updated date
  - Effective date
  - Developer contact information
  - Clickable links to YouTube ToS and Google Privacy Policy
- **Style:** Clean readable page with the same dark theme
- **URL:** `/Privacy` — this URL goes into the Play Store listing as `https://ytshortsalarm.kardsen.com/Privacy`

---

## 6. Assets Required

### From App Repository
| Asset | Source | Status |
|-------|--------|--------|
| App icon (512×512 PNG) | Needs export from `ic_launcher` | ⬜ TODO |
| App icon (SVG preferred) | Convert from adaptive icon | ⬜ TODO |
| Screenshots (7 images) | Provided by user (attached) | ✅ Available |

### To Create
| Asset | Specification | Owner |
|-------|--------------|-------|
| Phone mockup frames | CSS-only or SVG device frames wrapping screenshots | Frontend Developer |
| Open Graph image | 1200×630 PNG for social sharing | Frontend Developer |
| Favicon set | 16×16, 32×32, 180×180 (apple-touch), 192×192, 512×512 | Frontend Developer |
| Google Play badge | Official badge from Google (https://play.google.com/intl/en_us/badges/) | Frontend Developer |

---

## 7. SEO & Meta Tags

```html
<title>YT Shorts Alarm — Wake Up to YouTube Shorts | Android App</title>
<meta name="description" content="YT Shorts Alarm plays the latest YouTube Short from any channel as your alarm. Free, private, no ads. Available for Android 10+.">
<meta name="keywords" content="youtube shorts alarm, youtube alarm clock, wake up youtube, android alarm app, youtube shorts, morning alarm youtube">

<!-- Open Graph -->
<meta property="og:title" content="YT Shorts Alarm — Wake Up to YouTube Shorts">
<meta property="og:description" content="Turn any YouTube channel's Shorts into your morning alarm. Free, private, no tracking.">
<meta property="og:image" content="/images/og-image.png">
<meta property="og:url" content="https://ytshortsalarm.kardsen.com">
<meta property="og:type" content="website">

<!-- Twitter Card -->
<meta name="twitter:card" content="summary_large_image">
<meta name="twitter:title" content="YT Shorts Alarm — Wake Up to YouTube Shorts">
<meta name="twitter:description" content="Turn any YouTube channel's Shorts into your morning alarm.">
<meta name="twitter:image" content="/images/og-image.png">

<!-- Canonical -->
<link rel="canonical" href="https://ytshortsalarm.kardsen.com">
```

---

## 8. Project Structure

```
youtube_short_alarm_website/
├── SOLUTION.md                          # This document
├── YTShortsAlarm.Web/                   # ASP.NET Core Razor Pages project
│   ├── YTShortsAlarm.Web.csproj         # Project file (.NET 9.0)
│   ├── Program.cs                       # App startup, DI, middleware pipeline
│   ├── appsettings.json                 # Configuration (connection strings, app settings)
│   ├── appsettings.Development.json     # Dev overrides
│   │
│   ├── Data/
│   │   ├── AppDbContext.cs              # EF Core DbContext (SQLite)
│   │   └── WaitlistEntry.cs             # Entity: email signups
│   │
│   ├── Pages/
│   │   ├── _ViewImports.cshtml          # Tag helpers, namespaces
│   │   ├── _ViewStart.cshtml            # Default layout
│   │   ├── Index.cshtml                 # Landing page (all sections)
│   │   ├── Index.cshtml.cs              # Page model (handles email signup POST)
│   │   ├── Privacy.cshtml               # Privacy policy page
│   │   ├── Privacy.cshtml.cs            # Privacy page model
│   │   ├── Error.cshtml                 # Error page
│   │   ├── Error.cshtml.cs              # Error page model
│   │   └── Shared/
│   │       ├── _Layout.cshtml           # Shared layout (head, nav, footer)
│   │       └── _ValidationScriptsPartial.cshtml
│   │
│   ├── wwwroot/
│   │   ├── favicon.ico
│   │   ├── css/
│   │   │   └── site.css                 # Custom styles (brand colors, components)
│   │   ├── js/
│   │   │   └── site.js                  # Carousel, accordion, smooth scroll
│   │   └── images/
│   │       ├── icon-512.png             # App icon
│   │       ├── og-image.png             # Open Graph social preview
│   │       ├── google-play-badge.png    # Official badge (for post-launch)
│   │       └── screenshots/
│   │           ├── 01-dashboard.png
│   │           ├── 02-channel-selection.png
│   │           ├── 03-history.png
│   │           ├── 04-edit-alarm.png
│   │           ├── 05-playback-overrides.png
│   │           ├── 06-settings.png
│   │           └── 07-alarm-playing.png
│   │
│   └── Properties/
│       └── launchSettings.json          # Dev server ports
│
├── YTShortsAlarm.Web.Tests/             # xUnit test project
│   ├── YTShortsAlarm.Web.Tests.csproj
│   └── Pages/
│       └── IndexPageTests.cs            # Tests for email signup, validation
│
└── YTShortsAlarm.Web.sln                # Solution file
```

---

## 9. Implementation Phases

### Phase 1: Project Scaffold & Data Layer (Backend Developer)
**Objective:** Create the ASP.NET Core Razor Pages project with the waitlist data model.

**Tasks:**
1. Create `YTShortsAlarm.Web.sln` solution and `YTShortsAlarm.Web.csproj` project targeting .NET 9.0
2. Add NuGet packages: `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design`
3. Create `AppDbContext` with `WaitlistEntry` entity (Id, Email, CreatedAt, IpAddress)
4. Create EF Core migration for SQLite database
5. Configure `Program.cs`: DbContext DI, HTTPS redirection, static files, Razor Pages, antiforgery
6. Create `_Layout.cshtml` shared layout with head (meta/SEO), nav bar, footer, Bootstrap 5.3 CDN
7. Create `Error.cshtml` error page
8. Create `appsettings.json` with SQLite connection string and placeholder settings
9. Create `launchSettings.json` with dev ports

**Acceptance Criteria:**
- `dotnet build` succeeds
- `dotnet run` serves a placeholder page at `https://localhost:5001`
- SQLite database is created on first run with `WaitlistEntry` table
- Layout renders with Bootstrap 5.3 dark theme

### Phase 2: Landing Page & Email Signup (Frontend Developer + Backend Developer)
**Objective:** Build the full landing page with all sections and the working email signup form.

**Frontend Tasks:**
1. Create `Index.cshtml` with all 7 sections (hero, how-it-works, features, screenshots, FAQ, final CTA, footer)
2. Create `css/site.css` with CSS custom properties for brand colors (see §4), dark/light mode via `prefers-color-scheme`
3. Implement responsive layout (mobile-first, breakpoints at 640px/1024px)
4. Create phone mockup CSS wrapper for screenshots
5. Implement screenshot carousel with swipe (mobile) + arrows (desktop)
6. Implement FAQ accordion (expand/collapse)
7. Implement smooth scroll for anchor links
8. Implement lightbox for screenshot enlargement
9. Create "Coming Soon" badge component with email signup form (email input + "Notify Me" button)
10. Add lazy loading for screenshot images

**Backend Tasks:**
1. Create `Index.cshtml.cs` page model with:
   - `OnGet()` — renders the page
   - `OnPostAsync()` — handles email signup form submission
   - Input model with `[EmailAddress]` validation + antiforgery
   - Duplicate email check
   - Success/error message via `TempData`
2. Store signups in SQLite via EF Core
3. Rate-limit signups (e.g., max 3 per IP per hour) to prevent abuse
4. Return to `/#download` anchor with success toast on valid submission

**Acceptance Criteria:**
- All 7 sections render correctly on mobile (375px), tablet (768px), desktop (1440px)
- Dark mode matches the app's coral/brown color scheme
- Email signup form works end-to-end (submit → store → success message)
- Invalid emails show validation error (client-side + server-side)
- Duplicate emails show friendly "already registered" message
- Antiforgery token is present on the form
- Form is CSRF-protected
- Lighthouse accessibility score ≥ 95

### Phase 3: Privacy Policy Page (Frontend Developer)
**Objective:** Create the standalone privacy policy page.

**Tasks:**
1. Create `Privacy.cshtml` with the verbatim privacy policy text (Appendix A)
2. Add web-specific additions: last updated date, effective date, developer contact info
3. Make YouTube ToS and Google Privacy Policy links clickable
4. Style consistently with `_Layout.cshtml` dark theme
5. Ensure the page is accessible at `/Privacy`

**Acceptance Criteria:**
- Privacy policy text matches the in-app version exactly (with web additions)
- All external links work and open in new tabs
- Page passes WCAG 2.1 AA reading standards
- URL is `https://ytshortsalarm.kardsen.com/Privacy` (for Play Store listing)

### Phase 4: SEO, Meta Tags & Assets (Frontend Developer)
**Objective:** Finalize all meta tags, structured data, and asset optimization.

**Tasks:**
1. Add all meta tags to `_Layout.cshtml` (see §7 — title, description, OG, Twitter Card, canonical)
2. Add JSON-LD structured data (SoftwareApplication schema, see §10)
3. Optimize screenshot images (compress, serve WebP with `<picture>` fallback)
4. Create favicon set from app icon (ico, 32px, 180px apple-touch, 192px, 512px)
5. Create Open Graph image (1200×630)
6. Add Google Play badge placeholder (swap when store URL available)
7. Add `robots.txt` and `sitemap.xml` (Razor Pages can generate sitemap)
8. Add YouTube API attribution in footer

**Acceptance Criteria:**
- OG image renders correctly when shared on Twitter/Facebook/LinkedIn
- Structured data validates in Google Rich Results Test
- `robots.txt` allows all crawlers; `sitemap.xml` lists `/` and `/Privacy`
- Total page weight < 2MB
- Lighthouse: performance ≥ 90, accessibility ≥ 95, SEO ≥ 95

### Phase 5: Testing (Test Engineer)
**Objective:** Write tests for the server-side logic.

**Tasks:**
1. Create `YTShortsAlarm.Web.Tests` xUnit project
2. Test email signup happy path (valid email → stored in DB)
3. Test email validation (invalid formats rejected)
4. Test duplicate email handling
5. Test rate limiting
6. Test antiforgery protection (POST without token → 400)
7. Integration test: GET `/` returns 200 with expected content
8. Integration test: GET `/Privacy` returns 200 with privacy policy content

**Acceptance Criteria:**
- All tests pass (`dotnet test`)
- ≥ 90% line coverage on page models and data layer

### Phase 6: Deployment to InterServer (Backend Developer / DevOps)
**Objective:** Deploy the app to InterServer hosting.

**Tasks:**
1. Publish as self-contained for target runtime (e.g., `linux-x64` or `win-x64`)
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained
   ```
2. If Linux VPS:
   - Configure Nginx as reverse proxy → Kestrel (port 5000)
   - Set up systemd service for auto-start
   - Install Let's Encrypt SSL via Certbot for `ytshortsalarm.kardsen.com`
3. If Windows shared hosting:
   - Deploy to IIS with web.config transform
   - Configure IIS site binding for the domain
   - Install SSL certificate
4. Configure DNS: CNAME or A record for `ytshortsalarm.kardsen.com` → InterServer IP
5. Verify HTTPS works end-to-end
6. Test all pages and email signup on production
7. Submit sitemap to Google Search Console

**Acceptance Criteria:**
- Site live at `https://ytshortsalarm.kardsen.com` with valid SSL
- Privacy policy accessible at `https://ytshortsalarm.kardsen.com/Privacy`
- Email signup works on production
- SQLite database is created and persists across app restarts
- HTTP → HTTPS redirect works
- All internal and external links work

### Phase 7: Code Review (Code Reviewer)
**Objective:** Final quality gate.

**Tasks:**
1. Review all Razor Pages, page models, and data layer code
2. Verify OWASP Top 10 compliance (XSS via Razor encoding, CSRF via antiforgery, SQL injection via EF Core parameterization)
3. Verify responsive design on mobile/tablet/desktop
4. Verify accessibility (WCAG 2.1 AA)
5. Verify SEO meta tags and structured data
6. Verify privacy policy completeness and YouTube API compliance

---

## 10. Structured Data (JSON-LD)

```json
{
  "@context": "https://schema.org",
  "@type": "SoftwareApplication",
  "name": "YT Shorts Alarm",
  "operatingSystem": "Android 10+",
  "applicationCategory": "UtilitiesApplication",
  "description": "An alarm clock that plays the latest YouTube Short from any channel. Free, private, no ads.",
  "offers": {
    "@type": "Offer",
    "price": "0",
    "priceCurrency": "USD"
  },
  "author": {
    "@type": "Organization",
    "name": "Kardsen"
  },
  "softwareVersion": "0.1.0",
  "screenshot": [
    "https://ytshortsalarm.kardsen.com/images/screenshots/01-dashboard.png",
    "https://ytshortsalarm.kardsen.com/images/screenshots/07-alarm-playing.png"
  ]
}
```

---

## 11. YouTube API Compliance (Website Requirements)

Per the YouTube API Services Terms:
1. **Attribution:** The footer must include: "This app uses YouTube API Services"
2. **ToS Link:** Must link to https://www.youtube.com/t/terms
3. **Google Privacy Policy Link:** Must link to https://www.google.com/policies/privacy
4. **Privacy Policy:** Must be publicly accessible and disclose YouTube API usage

These are already handled in the privacy policy page and footer specifications.

---

## 12. Performance Budget

| Metric | Target |
|--------|--------|
| First Contentful Paint | < 1.5s |
| Largest Contentful Paint | < 2.5s |
| Total page weight | < 2MB |
| JavaScript bundle | < 20KB (minified) |
| CSS bundle | < 30KB (minified) |
| Lighthouse Performance | ≥ 90 |
| Lighthouse Accessibility | ≥ 95 |
| Lighthouse SEO | ≥ 95 |

---

## 13. Content Copy (Draft)

### Hero Tagline Options (pick one)
1. "Wake up to your favorite YouTube Shorts" ← **recommended**
2. "Your alarm clock just got a lot more interesting"
3. "The alarm that plays YouTube, not beeps"
4. "Rise and shine with YouTube Shorts"

### App Description (for underneath tagline)
> YT Shorts Alarm plays the latest Short from any YouTube channel as your morning alarm. Choose different channels for different days. Customize volume, replays, and more. Works reliably — even after reboots and in battery saver mode.
>
> **Free. No ads. No tracking. No account required.**

### How It Works Steps
1. **Pick a channel** — "Enter a YouTube @handle, channel URL, or channel ID. Each alarm can have its own channel."
2. **Set your alarm** — "Choose the time, select which days, and fine-tune playback settings like volume, replays, and rising volume."
3. **Wake up inspired** — "When your alarm fires, the latest Short plays full-screen — even over your lock screen. Snooze for 5 minutes or dismiss."

---

## 14. Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| App not yet on Play Store | High | Medium | Use "Coming Soon" CTA with email signup; swap badge when published |
| Screenshots are from a phone (status bar visible) | Medium | Low | Crop status bars or use device mockup frames to mask |
| No app icon asset available yet | Medium | Medium | Extract from APK or generate from source `ic_launcher` |
| InterServer hosting config | Medium | Medium | Document Nginx/IIS reverse proxy setup; test locally first with `dotnet publish` |
| SQLite concurrency under load | Low | Low | Minimal traffic pre-launch; migrate to PostgreSQL if needed post-launch |
| YouTube "YT" abbreviation compliance | Low | High | App already renamed; website should consistently use "YT Shorts Alarm" not "YouTube Short Alarm" |
| Email signup spam | Medium | Medium | Rate limiting + antiforgery + optional CAPTCHA |

---

## 15. Out of Scope (v1)

- Blog / changelog
- Multi-language / i18n
- User accounts / login / admin panel
- iOS version promotion
- A/B testing
- Automated email marketing / drip campaigns
- App Store Optimization (ASO) — separate effort
- Video demo / promotional video
- CI/CD pipeline (manual publish for v1)
- Database migration to PostgreSQL (SQLite sufficient for pre-launch)

---

## 16. Summary

ASP.NET Core Razor Pages promotional website with 2 pages:
1. **Landing page** — showcases the app with 7 sections, "Coming Soon" pre-launch CTA with email waitlist signup
2. **Privacy policy** — legal requirement for Play Store and YouTube API compliance

**Tech stack:** ASP.NET Core 9.0 Razor Pages, Bootstrap 5.3, SQLite (EF Core), deployed to InterServer.  
**Domain:** `https://ytshortsalarm.kardsen.com`  
**Backend features:** Email waitlist signup with validation, rate limiting, and antiforgery.  
**Key deliverables:**
- Marketing landing page to build pre-launch buzz
- `/Privacy` URL required for the Play Store listing
- Email waitlist to notify users at launch

---

## Appendix A: Privacy Policy Text

Extracted verbatim from the app's `strings.xml` (`privacy_policy_body`). The `/Privacy` Razor Page should display this text with the web-specific additions noted below.

### Core Text (from app)

> YT Shorts Alarm does not collect, store, or transmit any personal data to external servers.
>
> All settings and alarm data are stored locally on your device. Your YouTube Data API key is stored in encrypted local storage and is never shared.
>
> This app uses YouTube API Services. Your use of YouTube content through this app is subject to:
>
> • YouTube Terms of Service
> https://www.youtube.com/t/terms
>
> • Google Privacy Policy
> http://www.google.com/policies/privacy
>
> No analytics, tracking, or advertising SDKs are included in this app.

### Web-Specific Additions

The web privacy policy page should wrap the above in a proper document with:

1. **Title:** "Privacy Policy — YT Shorts Alarm"
2. **Effective date:** "Effective: April 4, 2026"
3. **Last updated:** "Last updated: April 4, 2026"
4. **Website data collection addendum:** "This website collects email addresses only for the purpose of notifying users about app availability. Email addresses are stored securely and never shared with third parties. You may request removal of your email by contacting support@kardsen.com."
5. **Contact:** "For questions about this privacy policy, contact: support@kardsen.com"
6. **Developer:** "YT Shorts Alarm is developed by Kardsen."
7. All URLs should be rendered as clickable links opening in new tabs

## Appendix B: Screenshot Inventory

| # | Screenshot | Description | Provided |
|---|-----------|-------------|----------|
| 1 | Dashboard (v1) | Original dashboard with 3 alarms, history collapsed | ✅ |
| 2 | Dashboard (v2) | Updated dashboard with play button, channel chip, history count | ✅ |
| 3 | Dashboard + History | Scrolled view showing expanded history with 2 entries | ✅ |
| 4 | Edit Alarm | Time picker, days, playback overrides collapsed | ✅ |
| 5 | Playback Overrides | Expanded overrides: channel, replays, videos, duration, volume, vibration, rising volume | ✅ |
| 6 | Settings | Permissions status, actions, about section with YouTube attribution | ✅ |
| 7 | Alarm Playing | Full-screen player with Short playing, snooze/dismiss buttons | ✅ |
