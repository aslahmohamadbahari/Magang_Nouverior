# Analisis: Mengapa `stateMachine.Agent.isOnNavMesh` Bisa Tidak Aktif

## Penyebab Utama

### 1. **NavMesh Belum di-bake di Scene**
   - Jika NavMesh belum di-bake (Bake ? Window > AI > Navigation > Bake)
   - Agent tidak bisa berada pada NavMesh karena NavMesh tidak ada
   - `isOnNavMesh` akan return `false`

### 2. **Agent.enabled = false**
   - Di `NPCStateMachine.cs`, tidak ada yang di-set `Agent.enabled = true`
   - Jika agent tidak di-enable, `isOnNavMesh` akan selalu `false`
   - Agent perlu di-enable untuk berinteraksi dengan NavMesh

### 3. **Agent Di-disable Saat Runtime**
   - Jika code lain menonaktifkan agent atau component-nya
   - `isOnNavMesh` akan return `false`

### 4. **Posisi Agent Di Luar NavMesh Area**
   - Jika agent di-spawn di area yang tidak ter-bake sebagai NavMesh
   - Agent tidak akan berada pada NavMesh

### 5. **Agent Jatuh / Physics Error**
   - Jika agent terjatuh dari platform NavMesh
   - Atau ada physics issue yang membuat agent terpisah dari NavMesh
   - `isOnNavMesh` akan return `false`

### 6. **Incompatible Configuration**
   - Di `NPCStateMachine.cs`: `Agent.updatePosition = false` dan `Agent.updateRotation = false`
   - Ini diatur agar menggunakan CharacterController untuk movement
   - Tapi kalau tidak di-setup dengan benar, bisa cause issue

## Masalah di Code NPCTargetingState

```csharp
private void MoveToTarget(float deltaTime)
{
    if (stateMachine.Agent.isOnNavMesh)  // Bisa return false!
    {
        Debug.Log("Moving to target: " + GetHighestThreatWeightTarget().name);
        stateMachine.Agent.destination = GetHighestThreatWeightTarget().transform.position;
        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
    }
    
    // SELALU dijalankan, bahkan jika Agent tidak on NavMesh!
    stateMachine.Agent.velocity = stateMachine.Controller.velocity;
}
```

**Masalah**: Jika `isOnNavMesh` = false, agent tidak di-update tapi velocity tetap di-set.

## Solusi Rekomendasi

1. **Pastikan NavMesh ter-bake** di scene
2. **Enable Agent di Awake/Start**
3. **Tambah null check dan error handling** untuk Agent
4. **Refactor MoveToTarget** untuk handle kedua case (on NavMesh / not on NavMesh)
