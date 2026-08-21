# KI-RnB Pakistan (HNMPL) - LX3 HEV/ICE

파키스탄 HNMPL 공장 ABS 롤러벤치 검사 프로그램

## 프로젝트 정보

| 항목 | 내용 |
|------|------|
| 프로젝트 | KI-RnB_1208 |
| 대상 공장 | 파키스탄 HNMPL |
| 차종 | LX3 HEV / LX3 ICE |
| 백업 브랜치 | `backup/before-error-fix` |

## 변경 이력

### 2026-08-15 — LX3 차종 추가 및 에러 방어 코드

---

### 1. LX3 HEV/ICE 차종 추가

#### 1.1 ECU 통신 클래스 추가 (cls_ECUs.cs)

| 항목 | LX3 HEV | LX3 ICE |
|------|---------|---------|
| ECU 모델명 | MOBIS LX3 HEV | MOBIS LX3 ICE |
| ECU 타입 | iMEB2 (현대모비스) | MEB5_1 (현대모비스) |
| 통신 프로토콜 | CAN (UDS) | CAN (UDS) |
| Send ID | 0x7E7 | 0x7D1 |
| Receive ID | 0x7EF | 0x7D9 |
| SecurityAccess | 불필요 (bypass) | 불필요 (bypass) |
| 클래스명 | MOBIS_LX3H | MOBIS_LX3I |

#### 1.2 구현된 ECU 통신 기능 (14개)
1. Start_Communication — Extended Diagnostic Session 시작
2. Stop_Communication — Default Session 복귀
3. ECU_Reset — ECU 하드/소프트 리셋
4. ECU_Identification — ECU 버전/파트넘버 읽기
5. Read__DTC — Diagnostic Trouble Code 읽기
6. Clear_DTC — DTC 삭제
7. Check_Signals — 브레이크 스위치 신호 확인
8. WSS_Test — 4륜 속도센서 값 읽기
9. Tester_Present — 세션 유지 (Keep-alive)
10. Message_Falg — ECU 메시지 플래그 확인
11. Dynamic_Step — ABS Dynamic 단계별 제어
12. Dynamic_Auto — ABS Dynamic 자동 시퀀스
13. ESP_Step — ESC/ESP 단계별 제어
14. ESS_LampTest — ESS 경고등 테스트

#### 1.3 ECUs 라우팅 등록
- ECUs 클래스의 각 메서드에 LX3 HEV/ICE case 분기 추가

#### 1.4 Driving Curve 파일
- 파일: `bin\Debug\DCurve\Curve(LX3).crv`
- 33 Steps / 167초 / 최대 100 km/h

#### 1.5 UI 및 DB 등록
- fomSetup.cs: cbo_ECUs 콤보박스에 MOBIS LX3 HEV/ICE 항목 추가
- fomDebug.cs: cbo_ECUs 콤보박스 + ESP Step 처리 LX3 case 추가
- DB (KI-RnB.mdb): tbl_CarModel 테이블에 LX3 HEV/ICE 모델 데이터 등록

---

### 2. 런타임 에러 방어 코드 (25개 파일, +800 / -71 라인)

#### 2.1 Parse → TryParse 전환 (99건)
- 1차: int/double/float Parse → TryParse 81건
- 2차: fomDebug Convert.ToSingle 15건, fomCurve Convert.ToInt32 3건

#### 2.2 SelectedIndex / SelectedItem 방어 (25건)
- fomSetup: ComboBox SelectedIndex 범위 체크 18건
- fomDebug: SelectedIndex >= 0 체크 3건
- fom_Data: SelectedItem null 체크 2건
- fomCurve: CurrentRow null 체크 2건

#### 2.3 CrossThread 방어 (InvokeRequired + BeginInvoke)

| 파일 | 메서드 | 내용 |
|------|--------|------|
| fom_Main.cs | Indi_LogData | InvokeRequired + BeginInvoke 패턴 |
| fom_Main.cs | ABSB_LogData | 동일 |
| fom_Main.cs | PLC_Log_Data | 동일 |
| fom_Main.cs | ABSBScanHerz | 동일 |
| fom_Test.cs | Refresh_Data 외 6개 | 7개 메서드에 InvokeRequired 추가 |
| cls_ABSB.cs | ThreadABSB 외 | BeginInvoke + IsHandleCreated 4곳 |

#### 2.4 NullReference 방어
- clsDBSql.cs: DBs_Conn null 체크 (DS_Select, DT_Select, Execute, Create)

#### 2.5 빈 catch 블록 로그 추가 (36건)
- 전체 170개 catch 중 비어있는 36건에 `Logs.MakeLog_File(Log_His.Err_, ...)` 추가
- cls_Logs.cs ExceptionErr (재귀 방지용) 1건 제외

#### 2.6 ListBox 누적 방지
- lst_Logs, lst_PLCs, lst_ABSB, lst_Indi, lst_DLCs: 500개 초과 시 오래된 항목 자동 삭제
- 삭제된 로그는 파일(Log File.log)에 별도 기록됨

---

### 3. ⚡ 신규 기능: Vehicle Balance PLC 출력

차량 모델별 Balance 검사 여부(Y/N)를 DB에서 관리하고, 검사 시작 시 PLC에 자동 전송하는 기능 추가.

#### 데이터 흐름

```
[DB] tbl_CarModel.dbBalance ("Y"/"N")
  ↕ [UI] fomSetup → chk_balance 체크박스로 설정/조회
  ↓ [fom_Main] 모델 선택 시 DB에서 읽어서
  ↓ [PLC] PLC.DO.Vehicle_Balance → D562[13] 신호로 PLC에 전송
          (W/B Select 신호 D562[0~9]와 동시 전송)
```

#### 수정 파일

| 파일 | 내용 |
|------|------|
| clsDBSql.cs | `dbBalance` 프로퍼티 추가, strModel/str_List/Init/조회 4개/Insert/Update에 반영 |
| fomSetup.cs | SelectModel에서 chk_balance 표시, Add/Edit에서 dbBalance 저장 |
| fom_Main.cs | Sel_Vehicles에서 `PLC.DO.Vehicle_Balance` 설정 후 `PLC_Put_D562()`로 전송 |
| cls_PLCs.cs | `Vehicle_Balance` 프로퍼티 (D562[13]), PLC_562_Mapp 매핑 (기 구현) |
| fomSetup.Designer.cs | `chk_balance` CheckBox 컨트롤 (기 구현) |

---

### 4. 기타 수정
- **바코드 VIN 우선 적용**: `txtVinNo`를 `Key_Vehicles` 호출 전에 설정하여 `DoEvents` 경쟁 방지. 바코드 스캔 시 새 VIN이 항상 우선 적용
- **WSS 파싱 오프셋**: LX3 HEV(iMEB2) `Ident[14~17]`, LX3 ICE(MEB5_1) `Ident[15~18]` (응답 길이 차이)
- **NRC 0x78 pending 처리**: `clsNeoVI.cs`에서 `7F XX 78` 응답 시 실패 대신 다음 응답 대기. LX3 DTC Clear 시 X 판정 해결
- **PLC Cancel/Stop 시 VIN 숨김**: 검사 시작 전 Cancel/Stop 시 `FomFlash.VinNo_Hide()` 호출
- **dgv_List 더블클릭 에러 수정**: `CellDoubleClick` 이벤트 등록 제거 + `SelectResult`를 `BeginInvoke`로 재진입 방지
- **Cancel/Stop 행 표시**: `tbl_InfoData`에 `dbStopFlag` 컬럼 추가, Cancel/Stop 종료 시 StopFlag 저장, dgv_List에서 해당 행 VINNO 빨간색 표시
- **DB 컬럼명**: `dbBalance` → `dbCarBalance`로 DB 컬럼명과 일치하도록 수정
- **dgvModel CurrentRow null 체크**: fomSetup `dgvModel_CurrentCellChanged`에서 CurrentRow null 체크 추가
- **cboModel 드롭다운**: PLC Select 처리 시 `!cboModel.DroppedDown` 체크 추가
- **fomCurve 타이틀**: 새 커브 생성 시 타이틀바에 모델명 표시
- **CAN 멀티프레임 Return 설정**: `STD_CAN_Read()` First Frame 핸들러에서 긍정응답 시 `Return = true` 누락 수정. HEV DTC Read X 판정 해결
- **바코드 자릿수 검증**: 9자리 미만/17자리 초과 시 팝업 표시, DB 매칭 실패 시 스캔값 표시
- **icsNeoClass**: ConvertFromHex catch 타입 OverflowException 유지

---

### 2026-08-19 — CAN 멀티프레임 WSS 데이터 밀림 수정 및 교정 방어 코드

---

### 1. CAN 멀티프레임 Get_Data 인덱스 밀림 수정 (clsNeoVI.cs)

`Ret_SendMsgs`에서 multi-frame 응답 수신 시, First Frame에서 `Return = true`로 즉시 탈출하면서 CF(Consecutive Frame)가 NeoVI 버퍼에 잔류. 다음 호출 시 `ECU_Clear()`가 `FF_0 = ""`로 초기화한 상태에서 잔류 CF가 먼저 처리되어 Get_Data 인덱스가 6칸 밀리는 문제 수정.

- **증상**: WSS 값이 255, 203, 127, 255로 표시 (실제 속도값 대신 다른 센서 데이터가 읽힘)
- **원인**: First Frame 처리(case "1")에서 `FF_0`를 세팅한 후 `Get_Data`를 재조립하지 않아, CF 처리(case "2")에서 `FF_0=""`로 조립된 데이터가 그대로 사용됨
- **수정**: First Frame 처리 시 `FF_0` 세팅 직후 `Get_Data`를 재조립하는 코드 추가
- **영향 차종**: 전 차종 공통 (AD, DN8, TL, TM, HEV, LX3 등)

### 2. Load 교정 방어 코드 (fom_Load.cs)

| 위치 | 내용 |
|------|------|
| tmr_Cals_Tick | `Pedal.IsOpen == false` → MessageBox: "Indicator is not connected." → 교정 진행 안 함 |
| tmr_Cals_Tick | `TSet.Bongshin == 0` → MessageBox: "Indicator value is 0." → 교정 진행 안 함 |
| LoadDataSave | `time * (DiaM / 2) == 0` 방어: divisor 체크 후 0이면 Calc = 0 처리 |

### 3. Loss 교정 방어 코드 (fom_Loss.cs)

| 위치 | 내용 |
|------|------|
| Set2_Point | `RPM__1 == RPM__2` → MessageBox: "RPM values are identical." (1회) → return |
| Calibrations | `rpmErrorShown = false` 초기화 (재교정 시 플래그 리셋) |
| Aver_LogLoss | `count++` 누락 버그 수정 + `SpdS > 0` 조건 추가로 유효 항목만 카운트 |

### 4. 실시간 측정 방어 코드 (clsDAQmx.cs)

| 위치 | 내용 |
|------|------|
| Wheel_Loss | `Dia / 2 == 0` → BeginInvoke MessageBox: "Roller diameter is 0." (1회) → return |
| Wheel_Loss | `Load.Indi == 0` → BeginInvoke MessageBox: "Load calibration data is invalid." (1회) + 무보정 처리 |

> BeginInvoke 사용 이유: `Wheel_Loss`는 백그라운드 스레드(NIDAQmx_Run)에서 호출되므로 UI 스레드에 위임

### 5. 기타

- **clsBS205.cs**: `Main?.Prog_LogData` null-conditional 연산자 적용
- **배포용 원복**: `ABSBoard.Setting` 주석 해제, `OnwerDrv=1`

---

### 5. 사이드 이펙트 검증

| 항목 | 결과 |
|------|------|
| Invoke → BeginInvoke (4개 메서드) | 안전 |
| fom_Test InvokeRequired (7개 메서드) | 안전 |
| cls_ABSB BeginInvoke + IsHandleCreated | 안전 |
| clsDBSql null 체크 | 안전 |
| icsNeoClass catch 타입 | 복원 완료 |
| fomCurve TryParse | 낮은 위험 |
| fomSetup SelectedIndex 범위 체크 | 안전 |
| 빈 catch 로그 추가 (36건) | 안전 |
| ListBox 500개 제한 | 안전 |

---

### 2026-08-20 — ISO-TP 멀티프레임 CF 버퍼 구조 개선 (clsNeoVI.cs)

---

### 1. 증상

| 증상 | 차종 | 내용 |
|------|------|------|
| A | NX4 PE HEV (IDB2, 7E7/7EF) | ECU Identification 간헐 에러. CF 수신 중 다음 요청이 버스에 실림 → ECU 무시 → 타임아웃 |
| B | LX3 HEV | DTC Read 화면 X. 통신 정상이나 응답 조립 실패로 완료 판정 안 됨 |

### 2. 원인

CF(Consecutive Frame) 버퍼가 `CF_1`~`CF_F` 15칸 고정이고, `switch`에 `case "20"`이 없었음.

ISO-TP SN(Sequence Number)은 4비트(0~F)라 `21`…`2F` 다음 `20`으로 순환. 16번째 CF(SN=0)가 어느 case에도 안 걸려 폐기 → 17번째 CF(SN=1)가 `CF_1`을 덮어씀 → 응답이 약 111바이트(DTC 27개)를 넘으면 `Get_Data`가 `CAN_Len`에 도달 불가 → 증상 B.

이를 우회하려고 FF 분기에 `else { Return = true; }` 추가 → FF만 받으면 성공 판정 → CF 수신 전 대기 루프 탈출 → 증상 A.

**부수 결함**: 완료 판정 off-by-one (Get_Buf 첫 토큰이 FF_DL 하위바이트라 토큰 수가 항상 1 많음), Gap_Ofst 미사용으로 타임아웃 "요청 후 1초" 고정 (LX3 HEV 응답 2초에 잘림).

### 3. 수정 내용

| # | 내용 | 상세 |
|---|------|------|
| ① | CF 버퍼 구조 변경 | `FF_0`/`CF_1`~`CF_F` 16개 필드 → `StringBuilder Get_Buf` 1개. SN 인덱스 없이 순서대로 Append |
| ② | FF 분기 수정 | `else { Return = true; }` 제거. FF는 수신 시작일 뿐, 완료 판정은 CF 분기로 단일화 |
| ③ | CF 분기 수정 | SN별 switch 제거 → 무조건 Append. `Ret_Length(Get_Data) - 1 >= CAN_Len`으로 완료 판정 (-1은 FF_DL 토큰 보정) |
| ④ | ECU_Clear() | `CAN_Len = 0` 추가, `Get_Buf.Length = 0`으로 버퍼 초기화 |
| ⑤ | Ret_SendMsgs() 타임아웃 | `Gap_Ofst`를 `Get_Buf.Length` 변화 시마다 갱신 → "마지막 프레임 후 1초"로 변경 (N_Cr 개념) |

### 4. 실측 데이터

| 차종 | 응답 크기 | CF 개수 | DTC 개수 | 소요 시간 |
|------|-----------|---------|----------|-----------|
| LX3 ICE | 151 byte | 21개 | 37개 | — |
| LX3 HEV | 1,159 byte (FF_DL 0x487) | 165개 | 289개 | 2.0초 |
| NX4 C101 | 42 byte | 6개 | — | 0.1초 |

### 5. 검증

- **NX4 PE HEV**: 통과 (ECU 통신 전 항목 정상)
- **LX3 ICE / LX3 HEV**: 통과 (코드 리뷰 완료)

---

## LX3 ABS 검사 시퀀스 (33 Steps / 167 sec)

| Phase | 구간 | 주요 내용 | 시스템 |
|-------|------|-----------|--------|
| 1. ECU Initialize | 0~26s | ECU 연결, DTC Read/Clear | ECU |
| 2. WSS Test | 26~46s | 4륜 속도센서 검사 (5km/h) | ECU + PLC + NI |
| 3. Speed & Cruise | 46~101s | 40/80/100km/h 속도 검사, 크루즈 | PLC + NI (+ ECU) |
| 4. Brake & ABS | 101~126s | 드래그, 제동력, ABS Dynamic | PLC + NI (+ ECU) |
| 5. Reverse & Parking | 126~149s | 후진 5km/h, 주차브레이크 | PLC + NI |
| 6. Finish | 149~167s | DTC 최종 확인, ECU 해제 | ECU |

> 상세 시퀀스: `LX3_ABS_Test_Sequence.pptx` 참조
