# ANALISIS: Kendala Pergantian Animasi di UpdateAnimator

## Masalah Utama

### 1. **Floating Point Comparison Issue (KRITIS)**
```csharp
if (distanceToTarget == distance)  // MASALAH!
{
    stateMachine.isRepositioning = true;
}
```

**Kendala:**
- Membandingkan dua float dengan `==` sangat tidak reliable
- `distanceToTarget` real-time terus berubah setiap frame
- `distance` parameter mungkin tidak pernah sama persis dengan real-time distance
- Animasi tidak akan pernah berpindah ke repositioning state

**Contoh:**
- Frame 1: distanceToTarget = 5.0000001
- Frame 2: distanceToTarget = 5.0000002
- Parameter distance = 5.0f
- Condition `== 5.0f` tidak akan pernah true!

---

### 2. **Random Target Range Setiap Frame (MASALAH)**
```csharp
private float StrafeDirectionTiming(float strafeTime)
{
    strafeTime = Random.Range(3f, stateMachine.timeRepositioning);  // Called di Enter()
    return strafeTime;
}
```

**Kendala:**
- Di `Enter()`, random range di-generate sekali saja (baik)
- Tapi `GetTargetingRange()` kemungkinan di-call setiap frame
- Jika method ini generate random, akan berbeda setiap frame
- Animasi transisi menjadi inconsistent

---

### 3. **Tidak Ada Hysteresis/Tolerance untuk Repositioning Transisi**
```csharp
// Forward state
float distanceToTarget = 5.2f;
float distance = 5.0f;
if (distanceToTarget == distance)  // 5.2 != 5.0 ? false
    stateMachine.isRepositioning = true;

// Strafe state
strafeTimer -= deltaTime;
if (strafeTimer <= 0f)  // Kapan kembali ke forward?
```

**Kendala:**
- Tidak ada buffer zone untuk transisi smooth
- Bisa terjadi jitter antara forward/strafe state
- Animasi bisa flickering

---

### 4. **Logic Flow Tidak Jelas**
```csharp
UpdateAnimator(bool repositioning, float distance, float deltaTime)
{
    if (repositioning)  // Strafe mode
    {
        // Count down timer, update Right anim
    }
    else  // Forward mode
    {
        // Check if should enter repositioning
        // Update Forward anim
    }
}
```

**Kendala:**
- Kondisi masuk repositioning sangat ketat (`==`)
- Kondisi keluar repositioning hanya timer (tapi tidak clear)
- State machine bisa stuck

---

## Solusi Rekomendasi

### 1. **Gunakan Range/Tolerance Untuk Float Comparison**
```csharp
// Bukan: if (distanceToTarget == distance)
// Gunakan:
if (Mathf.Abs(distanceToTarget - distance) < toleranceRange)
{
    stateMachine.isRepositioning = true;
}
```

### 2. **Implement Hysteresis untuk Transisi**
```csharp
float ENTER_THRESHOLD = 0.5f;  // Enter repositioning jika di range
float EXIT_THRESHOLD = 2.0f;   // Exit repositioning jika jauh dari range

if (!isRepositioning && Mathf.Abs(distanceToTarget - distance) < ENTER_THRESHOLD)
{
    isRepositioning = true;
}
else if (isRepositioning && Mathf.Abs(distanceToTarget - distance) > EXIT_THRESHOLD)
{
    isRepositioning = false;
}
```

### 3. **Cache Target Range di Enter()**
```csharp
public override void Enter()
{
    stateMachine.currentTargetRange = GetTargetingRange();  // Sekali saja
    // Gunakan stateMachine.currentTargetRange di Tick()
}
```

### 4. **Perbaiki State Machine Transisi**
- Strafe ? Forward: Saat `strafeTimer <= 0`
- Forward ? Strafe: Saat `distanceToTarget` jauh dari optimal range
- Add debug log untuk track state changes

---

## Real-Time Distance Behavior

**Scenario Saat Ini (BERMASALAH):**
```
Frame 1: Distance = 5.3 ? Forward (trying to reach 5.0)
Frame 2: Distance = 5.15 ? Forward
Frame 3: Distance = 5.05 ? Forward (5.05 != 5.0, tidak trigger repositioning!)
Frame 4: Distance = 4.95 ? Forward (now too close, switch to backward)
Frame 5: Distance = 5.1 ? Forward (cycle repeat)

Result: Jitter, animasi tidak smooth!
```

**Scenario Dengan Tolerance (LEBIH BAIK):**
```
Frame 1: Distance = 5.3 ? Forward state
Frame 2: Distance = 5.15 ? Forward state
Frame 3: Distance = 5.05 ? [5.05 within tolerance?] ? Yes ? Strafe state!
Frame 4-N: Distance hovering 5.0±0.5 ? Strafe state (smooth)
```

---

## Next Step Implementation

1. Define `repositioningTolerance` field di NPCStateMachine
2. Refactor `UpdateAnimator` dengan threshold-based logic
3. Cache `currentTargetRange` di Enter() bukan setiap frame
4. Add debug visualization untuk track distance vs target range
