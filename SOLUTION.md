# YT Shorts Alarm — Website Solution Document

**Date:** April 4, 2026  
**Revised:** April 23, 2026 — Updated with real-world assumptions from app codebase audit  
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
| Q7 | Privacy policy source | Based on app's `PrivacyPolicyActivity` with required corrections — see corrected Appendix A. **Do not use the original `strings.xml` text verbatim; it contains materially false claims about data collection.** |
| Q8 | Analytics | TBD — recommend Plausible (privacy-friendly) |
| Q9 | Developer name | "Kardsen" |
| Q10 | Contact email | `techtravis@gmail.com` (confirmed in play-store-readiness.md) |
| Q11 | Hosting | **InterServer** shared/VPS hosting |
| Q12 | SEO keywords | "youtube shorts alarm", "wake up youtube", "youtube alarm clock android" |
| Q13 | Business model | **Freemium** — 30-day Pro trial on first install; one alarm free after trial; Pro upgrade available (see app store listing for current pricing) |
| Q14 | Firebase usage | Firebase Crashlytics (all users), Firebase Auth + Realtime Database (Friends feature, opt-in only) |
| Q15 | Deep links | App handles `ytshortsalarm.kardsen.com/alarm` and `/join` — website must serve both routes and `/.well-known/assetlinks.json` |

---

## 1. Project Overview

### Purpose
A promotional website for the **YT Shorts Alarm** Android app that:
1. Showcases the app's full feature set with screenshots
2. Drives downloads from the Google Play Store
3. Hosts the legally-required **Privacy Policy** (needed for Play Store listing)
4. Hosts the **YouTube API Services** compliance pages (ToS links, attribution)
5. Serves as the verified domain for **Android App Links** (`/alarm`, `/join` deep links)
6. Builds pre-launch buzz with an optional email signup

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
| **Privacy Policy** | `/privacy` | Legal requirement for Play Store + YouTube API compliance |
| **Alarm Deep Link** | `/alarm` | Web fallback for Android App Link; presents download CTA |
| **Join Deep Link** | `/join` | Web fallback for Friends group join link; presents download CTA |

> **URL casing:** Use `/privacy` (lowercase) throughout to match `play-store-readiness.md`. Configure `LowercaseUrls = true` in `Program.cs` and add a 301 redirect from `/Privacy` to `/privacy`.

### Sitemap

```
/                     → Landing page (Razor Page with anchor sections)
  /#hero              → Hero with app name, tagline, "Coming Soon" + email signup
  /#how-it-works      → 4-step explanation
  /#features          → Feature grid/cards
  /#screenshots       → Screenshot carousel/gallery
  /#faq               → Frequently asked questions
  /#download          → Final CTA with email signup
/privacy              → Privacy Policy (standalone Razor Page)
/alarm                → App deep link fallback — "Download YT Shorts Alarm"
/join                 → App deep link fallback — "Download & join group"
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

### Android App Backend Stack

The website's privacy policy, FAQ copy, and Data Safety form must accurately reflect the app's actual backend components. The following table is the authoritative reference.

| Layer | Technology | Version | Notes |
|-------|-----------|---------|-------|
| **Language** | Kotlin with coroutines | — | `StateFlow`-based reactive patterns throughout |
| **Billing** | Google Play Billing (`billing-ktx`) | 7.1.1 | One-time `pro_unlock` (INAPP) and monthly subscription (SUBS); 30-day Pro trial on new installs |
| **Firebase Auth** | Firebase Authentication (`firebase-auth-ktx`) | BOM 33.7.0 | Google Sign-In for Friends feature; opt-in only; stores Google account UID |
| **Firebase Realtime DB** | Firebase Realtime Database (`firebase-database-ktx`) | BOM 33.7.0 | Friends: group membership, display names, alarm event history (7-day rolling) |
| **Firebase Crashlytics** | Firebase Crashlytics (`firebase-crashlytics-ktx`) | BOM 33.7.0 | Crash/error reporting for **all users unconditionally**; collects device model, Android version, stack traces |
| **HTTP Client** | OkHttp | 4.12.0 | Fetches featured channel/video suggestions from promotional API; anonymous requests |
| **In-App Update** | Play Core App Update (`app-update-ktx`) | 2.1.0 | Prompts users for in-app updates via `AppUpdateManager` |
| **Local Storage** | SharedPreferences + EncryptedSharedPreferences | AndroidX | Alarms/history/settings in SharedPreferences; YouTube API key in AES-256-GCM encrypted storage |
| **Alarm Scheduling** | AlarmManager (exact) + BroadcastReceiver + ForegroundService | Android platform | Survives Doze mode, battery optimization, and reboots via `BootReceiver` |
| **YouTube Playback** | YouTube IFrame API via WebView | — | Plays Shorts; no video downloading or caching |

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
- **Visual:** Phone mockup showing the alarm player screen (screenshot 7)
- **Background:** Dark gradient matching app's surface color
- **Pre-launch promise line:** "Try free — 30-day Pro trial included. No ads."
- **Email Signup Form:** Simple inline form — email input + "Notify Me" button, server-side POST to `/` handler

#### Section 2: How It Works
- **Layout:** 4-step horizontal cards
- **Content:**
  1. **Pick a channel** — "Enter any YouTube @handle, channel URL, or channel ID. Each alarm can have its own channel."
  2. **Set your alarm** — "Choose your time, select which days, and configure Playback Overrides. Pro unlocks unlimited alarms."
  3. **Wake up (and prove it)** — "The latest Short plays full-screen, even over your lock screen. Complete your dismiss challenge to stop the alarm."
  4. **Track your progress** — "View wake streaks, snooze rate, and achievement badges. Join friend groups for accountability."
- **Icons:** Material Symbols (play_circle, alarm, smartphone, bar_chart)

#### Section 3: Features Grid
- **Layout:** Responsive grid — 2 columns on tablet, 3 columns on desktop (11 cards in 3–4 rows; last row centered). Mobile: single column.
- **Cards:**

  1. **Smart Video Selection** — "Automatically finds the newest embeddable Short from any YouTube channel. Enter a @handle, channel URL, or channel ID."

  2. **Reliable Delivery** — "Uses Android's exact alarm API — survives Doze mode and reboots. Falls back to your ringtone within 15 seconds if there's no internet."

  3. **Dismiss Challenges** ⭐Pro — "Actually have to get up. Solve a math problem, shake your phone, scan a QR code, scan a barcode, or type a custom phrase before the alarm stops."

  4. **Wake Confirmation** — "After you dismiss, the app uses your motion sensor to check you've gotten up. If no movement is detected, the alarm re-fires."

  5. **Sleep Timer** — "Wind down before bed with YouTube Shorts. Set a countdown timer and volume fades out gradually while you fall asleep."

  6. **Bedtime Reminder** — "Set a wind-down reminder. The app nudges you when it's time to put the phone down and head to bed."

  7. **Statistics & Achievements** — "Track wake streaks, weekly alarm charts, snooze rate, and average wake time. Earn from 21 achievement badges."

  8. **Friends & Accountability** — "Create or join accountability groups with an 8-character invite code. See friends' alarm statuses in real time. (Requires Google Sign-In.)"

  9. **Multiple Alarms & Scheduling** ⭐Pro for unlimited — "Set alarms by day of week, every N days, or with specific date skips. Each alarm has its own channel, volume, replays, playlist count, and rising volume."

  10. **Playlist Mode** ⭐Pro — "Queue up to 10 YouTube Shorts per alarm. Blocked videos are skipped automatically."

  11. **Home Screen Widgets** — "Four widget types: upcoming alarm, dashboard summary, wake streak, and friends' accountability status."

  12. **Rising Volume** — "Alarms can start quietly and gradually increase to full volume, letting you wake up gently instead of being startled. Ramp duration and target volume are configurable per alarm."

- **Each card:** Icon + title + 1-2 sentence description. Pro-gated features marked with ⭐Pro.
- **Layout:** Responsive grid — 2 columns on tablet, 3 columns on desktop (12 cards in 4 rows; last row centered). Mobile: single column.

#### Section 4: Screenshots
- **Layout:** Horizontal scrollable carousel (mobile) / grid (desktop)
- **Screenshots:** All 7 provided images displayed in phone mockup frames
- **Labels under each:**
  1. "Alarm dashboard"
  2. "Updated dashboard"
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
  - Android AlarmManager exact alarms — Doze and reboot resistant
  - Encrypted local storage (AES-256-GCM via EncryptedSharedPreferences)
  - YouTube Data API v3 integration
  - Firebase Realtime Database (Friends real-time status)
  - Firebase Crashlytics (crash reporting — all users)
  - 4 home screen widget types
  - 21 achievement badges
  - Export / import alarm configuration
  - 51+ unit tests (JUnit4 + Robolectric + MockK)

#### Section 6: FAQ
- **Layout:** Accordion (expand/collapse)
- **Questions:**
  1. "Is YT Shorts Alarm free?" → "Core features are free to download with no account required. All new installs include a 30-day Pro trial so you can experience everything. After the trial, you can keep using one alarm for free or upgrade to Pro. No ads."
  2. "What's included in Pro?" → "Pro removes the one-alarm limit so you can set unlimited alarms. It also unlocks Dismiss Challenges (math, shake, QR scan, barcode, typing), extended Playlist Mode (up to 10 videos per alarm), and priority access to new features."
  3. "Is there a free trial?" → "Yes — every new install automatically starts a 30-day Pro trial. No card required. You get full access to all Pro features during the trial."
  4. "Do I need a Google account?" → "No account is required for alarms, scheduling, or playback. A Google account is only needed if you want to use the Friends accountability feature."
  5. "Do I need a YouTube API key?" → "Yes. The app fetches videos through the YouTube Data API v3. You'll need a free API key from Google Cloud Console — setup takes about 2 minutes and the free quota is more than enough for daily alarm use."
  6. "What happens if I have no internet when the alarm fires?" → "The app falls back to your device's default alarm tone within 15 seconds. Your alarm will always go off, even with no signal or no API quota remaining."
  7. "Does it work with any YouTube channel?" → "Yes — enter a @handle, channel URL, or channel ID. The app finds the latest embeddable Short-style video from that channel. Blocked or age-restricted videos are skipped automatically."
  8. "Will my alarm go off after a reboot?" → "Yes. All alarms are rescheduled automatically the moment your device restarts, before you even unlock it."
  9. "Does the app drain my battery?" → "No. The app requests battery optimization bypass — a standard permission for alarm clocks — so Android doesn't defer your alarm. The app does nothing between alarms; it relies entirely on Android's hardware-backed AlarmManager."
  10. "Why does the app need so many permissions?" → "Each permission serves a specific alarm function: exact alarm timing (Android 12+ requires explicit permission), waking the screen over the lock screen, showing full-screen notifications, surviving battery optimization, and rescheduling after reboot. On Android 14+, the notification permission prompt appears at first launch."

#### Section 7: Final CTA
- **Layout:** Centered, full-width dark section
- **Content:**
  - H2: "Ready to wake up differently?"
  - Email signup form (same as hero — "Notify Me" for pre-launch)
  - "30-day Pro trial included. Core features always free."
  - When app launches: swap signup for Google Play badge

#### Footer
- App name + tagline
- Developer: "Made by Kardsen"
- Links: Privacy Policy | Contact
- Contact email: `techtravis@gmail.com`
- YouTube API attribution: "This app uses YouTube API Services. By using this app you agree to be bound by the YouTube Terms of Service."
- Links: YouTube Terms of Service | Google Privacy Policy

### 5b. Privacy Policy Page (`/privacy`)

- **Content:** Use the **corrected privacy policy** from Appendix A. **Do not** use the in-app `strings.xml` text verbatim — it contains materially false claims about data collection that are contradicted by Firebase Crashlytics being active in all builds.
- **Additions for web:**
  - Last updated date
  - Effective date
  - Developer contact: `techtravis@gmail.com`
  - Clickable links to YouTube ToS, Google Privacy Policy, and Firebase Privacy and Security
- **Style:** Clean readable page with the same dark theme
- **URL:** `/privacy` (lowercase) — configure `LowercaseUrls = true` in `Program.cs` and add a 301 redirect from `/Privacy` → `/privacy`. Update the Play Store listing to use `https://ytshortsalarm.kardsen.com/privacy`.

### 5c. Deep Link Fallback Pages (`/alarm`, `/join`)

Android App Links allow the Android OS to open `ytshortsalarm.kardsen.com/alarm` and `ytshortsalarm.kardsen.com/join` directly in the app without a browser chooser. The website must support three requirements:

#### Digital Asset Links (`/.well-known/assetlinks.json`)

The file proves domain ownership and must be served at exactly this path with `Content-Type: application/json`. Use the **SHA-256** certificate fingerprint from Play Console (Setup → App integrity → App signing key certificate). SHA-1 is not accepted.

```json
[{
  "relation": ["delegate_permission/common.handle_all_urls"],
  "target": {
    "namespace": "android_app",
    "package_name": "com.kardsen.ytshortsalarm",
    "sha256_cert_fingerprints": ["<SHA-256 from Play Console — to be filled post-setup>"]
  }
}]
```

In `Program.cs`, configure static file serving to allow `.json` files under `.well-known/`:

```csharp
app.UseStaticFiles(new StaticFileOptions {
    ContentTypeProvider = new FileExtensionContentTypeProvider {
        Mappings = { [".json"] = "application/json" }
    }
});
```

#### `/alarm` Razor Page

Web fallback shown when the user does not have the app installed (e.g., on a desktop browser or a fresh Android device before the app is installed).

- **H1:** "Open in YT Shorts Alarm"
- **Body:** "This link is designed to open in the YT Shorts Alarm Android app. If you don't have the app yet, download it below."
- **CTA:** Google Play badge → Play Store URL

#### `/join` Razor Page

Web fallback for Friends group invite links.

- **H1:** "You've been invited!"
- **Body:** "You've been invited to an accountability group in YT Shorts Alarm. Download the app to join."
- **CTA:** Google Play badge → Play Store URL

#### Web Server Configuration

For **Nginx**, ensure `.well-known` is not blocked:

```nginx
location /.well-known/ {
    allow all;
}
```

For **IIS** (`web.config`), ensure the rewrite rules do not intercept `/.well-known/` paths. Static file serving must take priority over Kestrel routing for this path. Verify the file is accessible with a browser after deployment.

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
<meta name="description" content="YT Shorts Alarm plays the latest YouTube Short from any channel as your alarm. No account required. No behavioral tracking. Available for Android 10+.">
<meta name="keywords" content="youtube shorts alarm, youtube alarm clock, wake up youtube, android alarm app, youtube shorts, morning alarm youtube">

<!-- Open Graph -->
<meta property="og:title" content="YT Shorts Alarm — Wake Up to YouTube Shorts">
<meta property="og:description" content="Turn any YouTube channel's Shorts into your morning alarm. No account required. Core features free to download.">
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
│   │   ├── Alarm.cshtml                 # Deep link fallback — app download CTA
│   │   ├── Alarm.cshtml.cs              # Alarm page model
│   │   ├── Join.cshtml                  # Deep link fallback — group join CTA
│   │   ├── Join.cshtml.cs               # Join page model
│   │   ├── Error.cshtml                 # Error page
│   │   ├── Error.cshtml.cs              # Error page model
│   │   └── Shared/
│   │       ├── _Layout.cshtml           # Shared layout (head, nav, footer)
│   │       └── _ValidationScriptsPartial.cshtml
│   │
│   ├── wwwroot/
│   │   ├── favicon.ico
│   │   ├── .well-known/
│   │   │   └── assetlinks.json          # Android App Links verification (SHA-256 fingerprint)
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
│   │           ├── 02-dashboard-v2.png
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
10. Create `wwwroot/.well-known/assetlinks.json` stub file with placeholder SHA-256 fingerprint (to be filled from Play Console after app signing is configured)
11. Configure `StaticFileOptions` in `Program.cs` to serve `application/json` for files under `.well-known/` and ensure `UseStaticFiles` is called before routing

**Acceptance Criteria:**
- `dotnet build` succeeds
- `dotnet run` serves a placeholder page at `https://localhost:5001`
- SQLite database is created on first run with `WaitlistEntry` table
- Layout renders with Bootstrap 5.3 dark theme
- `GET /.well-known/assetlinks.json` returns `200 application/json`

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
11. Create `Alarm.cshtml` — deep link fallback page: "This link opens in YT Shorts Alarm. Download the app to continue." + Google Play badge
12. Create `Join.cshtml` — group invite fallback page: "You've been invited to an accountability group! Download YT Shorts Alarm to join." + Google Play badge

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
5. Ensure the page is accessible at `/privacy` (lowercase)

**Acceptance Criteria:**
- Privacy policy text matches the in-app version exactly (with web additions)
- All external links work and open in new tabs
- Page passes WCAG 2.1 AA reading standards
- URL is `https://ytshortsalarm.kardsen.com/privacy` (lowercase — for Play Store listing)
- `/Privacy` (uppercase) redirects with 301 to `/privacy`

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
- `robots.txt` allows all crawlers; `sitemap.xml` lists `/`, `/privacy`, `/alarm`, and `/join`
- `/.well-known/assetlinks.json` returns `200 application/json` (verify from Phase 1 implementation)
- Total page weight < 2MB
- Lighthouse: performance ≥ 90, accessibility ≥ 95, SEO ≥ 95

### Phase 5: Testing (Test Engineer)
**Objective:** Write tests for the server-side logic of the **website** (`YTShortsAlarm.Web`).

> **Scope note:** These are website integration/unit tests only. The Android app has a separate unit test suite (51 tests, JUnit4 + Robolectric + MockK) covering URL parsing, alarm scheduling, retry logic, settings serialization, and video selection. Android app tests are not part of this phase.

**Tasks:**
1. Create `YTShortsAlarm.Web.Tests` xUnit project
2. Test email signup happy path (valid email → stored in DB)
3. Test email validation (invalid formats rejected)
4. Test duplicate email handling
5. Test rate limiting
6. Test antiforgery protection (POST without token → 400)
7. Integration test: GET `/` returns 200 with expected content
8. Integration test: GET `/privacy` returns 200 with privacy policy content
9. Integration test: GET `/Privacy` returns 301 redirect to `/privacy`
9. Integration test: GET `/alarm` returns 200 with app download content
10. Integration test: GET `/join` returns 200 with app download content
11. Integration test: GET `/.well-known/assetlinks.json` returns 200 with `Content-Type: application/json`

**Acceptance Criteria:**
- All tests pass (`dotnet test`)
- ≥ 90% line coverage on page models and data layer
- Routes `/alarm`, `/join`, and `/.well-known/assetlinks.json` each return the expected status code and content type

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
   - Add Nginx rule to allow `/.well-known/` path (required for Android App Links verification and Let's Encrypt challenges):
     ```nginx
     location /.well-known/ {
         allow all;
     }
     ```
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
- Privacy policy accessible at `https://ytshortsalarm.kardsen.com/privacy`
- `/Privacy` (uppercase) returns 301 redirect to `/privacy`
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
7. Verify deep link fallback pages: `GET /alarm` and `GET /join` each return 200 with Google Play badge and download CTA
8. Verify `GET /.well-known/assetlinks.json` returns `200 application/json` (not 404, not text/html)
9. Verify `/Privacy` (uppercase) returns a 301 redirect to `/privacy` (lowercase)

---

## 10. Structured Data (JSON-LD)

```json
{
  "@context": "https://schema.org",
  "@type": "SoftwareApplication",
  "name": "YT Shorts Alarm",
  "operatingSystem": "Android 10+",
  "applicationCategory": "UtilitiesApplication",
  "description": "An alarm clock that plays the latest YouTube Short from any channel as your morning alarm. Includes dismiss challenges, sleep timer, wake tracking, and an optional friends accountability group. Core features free; Pro upgrade available.",
  "offers": {
    "@type": "AggregateOffer",
    "lowPrice": "0",
    "priceCurrency": "USD",
    "offerCount": "2",
    "offers": [
      {
        "@type": "Offer",
        "name": "Free tier",
        "price": "0",
        "priceCurrency": "USD"
      },
      {
        "@type": "Offer",
        "name": "Pro upgrade",
        "description": "See app\u2019s Play Store listing for current pricing",
        "priceCurrency": "USD"
      }
    ]
  },
  "author": {
    "@type": "Organization",
    "name": "Kardsen"
  },
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
> YT Shorts Alarm plays the latest Short from any YouTube channel as your morning alarm. Set dismiss challenges, track your wake streaks, and invite friends for accountability. Customize volume, replays, playlist counts, and more. Works reliably — even after reboots and in battery saver mode.
>
> **30-day Pro trial included. Core features always free. No ads.**

### How It Works Steps
1. **Pick a channel** — "Enter a YouTube @handle, channel URL, or channel ID. Each alarm can have its own channel."
2. **Set your alarm** — "Choose the time, select which days, and fine-tune playback settings like volume, replays, playlist count, and rising volume. Pro unlocks unlimited alarms."
3. **Wake up (and prove it)** — "When your alarm fires, the latest Short plays full-screen — even over your lock screen. Complete your dismiss challenge to stop it."
4. **Track your progress** — "View wake streaks and achievement badges in Statistics. Invite friends for group accountability."

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
| Misleading pricing copy | High | High | Never state "completely free" or "no in-app purchases" — the app has a freemium model. Reference "see app store listing" for pricing |
| Misleading privacy copy | High | High | Do not use the original `strings.xml` privacy text. Use the corrected Appendix A, which accurately discloses Firebase Crashlytics on all users |
| App Links not working on fresh install | Medium | Medium | The OS opens the web fallback when app is not installed; `/alarm` and `/join` Razor Pages provide a smooth download CTA |
| `assetlinks.json` SHA fingerprint wrong | Medium | High | Always use SHA-256 (not SHA-1); obtain from Play Console → App integrity; test verification with `adb shell pm verify-app-links --re-verify` |
| Pricing on website becomes stale | Medium | Medium | Do not hard-code a price; use "see the app’s Play Store listing for current pricing" to avoid stale copy after any price change |

---

## 15. Out of Scope (v1)

- Blog / changelog
- Multi-language / i18n
- User accounts for the **website** / login / admin panel *(note: the Android app itself uses Google Sign-In for the opt-in Friends feature; this is an app-level account, not a website-level account)*
- iOS version promotion
- A/B testing
- Automated email marketing / drip campaigns
- App Store Optimization (ASO) — separate effort
- Video demo / promotional video
- CI/CD pipeline (manual publish for v1)
- Database migration to PostgreSQL (SQLite sufficient for pre-launch)

---

## 16. Summary

ASP.NET Core Razor Pages promotional website with **4 pages**:
1. **Landing page** (`/`) — showcases the app with 7 sections, "Coming Soon" pre-launch CTA with email waitlist signup
2. **Privacy policy** (`/privacy`) — legal requirement for Play Store and YouTube API compliance; uses the corrected Appendix A text (not the original `strings.xml` text)
3. **Alarm deep link fallback** (`/alarm`) — web fallback for Android App Link; presents download CTA
4. **Join deep link fallback** (`/join`) — web fallback for Friends group invite link; presents download CTA

**Tech stack:** ASP.NET Core 9.0 Razor Pages, Bootstrap 5.3, SQLite (EF Core), deployed to InterServer.  
**Domain:** `https://ytshortsalarm.kardsen.com`  
**Backend features:** Email waitlist signup with validation, rate limiting, and antiforgery. Digital Asset Links verification via `/.well-known/assetlinks.json`.  
**Key deliverables:**
- Marketing landing page to build pre-launch buzz
- `/privacy` URL required for the Play Store listing (lowercase; add 301 redirect from `/Privacy`)
- Email waitlist to notify users at launch
- Deep link infrastructure for Android App Links (`/alarm`, `/join`, `/.well-known/assetlinks.json`)
- Corrected freemium-accurate content: 30-day Pro trial, no false "completely free" claims, no false "zero tracking SDKs" claims

---

## Appendix A: Privacy Policy Text (Corrected)

> ⚠️ **Do not use the original `strings.xml` `privacy_policy_body` text.** It states “No analytics, tracking, or advertising SDKs are included in this app” — which is contradicted by Firebase Crashlytics being included in all builds (unconditionally). Use the corrected text below.

---

### Privacy Policy — YT Shorts Alarm

**Effective:** April 4, 2026  
**Last updated:** April 23, 2026  
**Developer:** Kardsen — techtravis@gmail.com

---

#### Data Collected by This App

**All users (no account required):**

- **Firebase Crashlytics** — If the app crashes or encounters a non-fatal error, Firebase Crashlytics automatically collects: device model, Android version, app version, crash stack trace, and a non-personally-identifiable installation identifier. This data is sent to Google’s Firebase servers.

**Users who enable the Friends feature (optional, requires Google Sign-In):**

- **Google Account** — Your Google account UID and display name are stored in Firebase Realtime Database to identify you within accountability groups.
- **Alarm event data** — Timestamps for alarm dismissals and snoozes are visible to members of your accountability group for 7 rolling days. No alarm channel, video, or content data is shared.

**All other data — stored locally on-device only:**

- Alarm schedules, channels, settings, and history are stored locally in Android SharedPreferences and are never transmitted to any server.
- Your YouTube Data API key is stored in AES-256-GCM encrypted local storage (EncryptedSharedPreferences) and is never shared.

---

#### YouTube API Services

This app uses YouTube API Services. Your use of YouTube content through this app is subject to:

- [YouTube Terms of Service](https://www.youtube.com/t/terms)
- [Google Privacy Policy](https://policies.google.com/privacy)
- [Firebase Privacy and Security](https://firebase.google.com/support/privacy)

---

#### Website Data Collection

This website (ytshortsalarm.kardsen.com) collects email addresses only for the purpose of notifying users about app availability. Email addresses are stored securely and are never shared with third parties. You may request removal of your email by contacting techtravis@gmail.com.

---

#### Contact

For questions about this privacy policy:  
**Email:** techtravis@gmail.com  
**Developer:** YT Shorts Alarm is developed by Kardsen.

## Appendix B: Screenshot Inventory

| # | File | Label | Description | Status |
|---|------|-------|-------------|--------|
| 1 | `01-dashboard.png` | "Alarm dashboard" | Original alarm dashboard with multiple alarms | ✅ Provided |
| 2 | `02-dashboard-v2.png` | "Updated dashboard" | Updated dashboard with play button, channel chip, and history summary | ✅ Provided |
| 3 | `03-history.png` | "Alarm history" | Dashboard scrolled to show expanded history with playback entries | ✅ Provided |
| 4 | `04-edit-alarm.png` | "Edit alarm" | Alarm edit screen: time picker, days of week, Playback Overrides collapsed | ✅ Provided |
| 5 | `05-playback-overrides.png` | "Playback overrides" | Expanded overrides: channel, replays, video count, duration, volume, vibration, rising volume | ✅ Provided |
| 6 | `06-settings.png` | "Settings & permissions" | Settings screen showing permissions status, app actions, and YouTube attribution | ✅ Provided |
| 7 | `07-alarm-playing.png` | "Alarm playing" | Full-screen alarm player with Short playing, snooze and dismiss buttons | ✅ Provided |

> **Note:** The legacy filename `02-channel-selection.png` is incorrect. The screenshot shows the updated dashboard (v2), not a channel selection screen. Use `02-dashboard-v2.png` as the canonical filename.

---

## Appendix C: Play Store Data Safety Declarations

The following maps the app’s actual data collection to the Play Store Data Safety form. Update whenever Firebase usage changes.

### Data Collected

| Data type | Category | Collected by | Purpose | All users? | User control |
|-----------|----------|-------------|---------|-----------|-------------|
| Crash logs (stack traces, device model, OS version) | App info and performance | Firebase Crashlytics | App functionality | Yes | No opt-out in current version |
| Google account UID + display name | Personal info (name, user IDs) | Firebase Auth + Realtime DB | Friends feature | No — Friends only | User may sign out to stop sharing |
| Alarm event timestamps (dismiss/snooze) | App activity | Firebase Realtime DB | Friends accountability | No — Friends only | Visible only to group members; deleted after 7 days |

### Data NOT Collected

- No advertising ID
- No precise or approximate location
- No contacts
- No financial info (payments processed by Google Play, not the app)
- No YouTube watch history
- No YouTube API queries (calls use the user’s own API key and quota)

### Data Sharing

| Shared with | Purpose |
|------------|--------|
| Google (Firebase) | Crash reporting (Crashlytics, all users) + Friends feature (Auth + Realtime DB, opt-in only) |
| Google (Play Billing) | In-app purchase processing |

### Security Practices

- Data encrypted in transit (Firebase uses TLS)
- YouTube API key encrypted at rest (AES-256-GCM, EncryptedSharedPreferences)
- No server-side storage of alarm schedules, channels, or content outside of Friends event sync
