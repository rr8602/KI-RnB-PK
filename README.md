# KI-RnB Pakistan (HNMPL) - LX3 HEV/ICE

파키스탄 HNMPL 공장 ABS/ESC 롤러벤치 검사 프로그램

## 프로젝트 정보

| 항목 | 내용 |
|------|------|
| 프로젝트 | KI-RnB_1208 |
| 대상 공장 | 파키스탄 HNMPL |
| 차종 | LX3 HEV / LX3 ICE |
| 백업 브랜치 | `backup/before-error-fix` |

## 변경 이력

### 2026-08-15 — LX3 차종 추가 및 에러 방어 코드

#### 1. LX3 HEV/ICE 차종 추가
- LX3 Driving Curve 파일 추가 (`Curve(LX3).crv`)
- DB 모델 데이터 등록

#### 2. 런타임 에러 방어 코드 (25개 파일, +800 / -71 라인)

##### 2.1 Parse → TryParse 전환 (99개소)
- 1차: int/double/float Parse → TryParse 81개소
- 2차: fomDebug Convert.ToSingle 15개소, fomCurve Convert.ToInt32 3개소

##### 2.2 SelectedIndex / SelectedItem 방어 (25개소)
- fomSetup: ComboBox SelectedIndex 범위 체크 18개소
- fomDebug: SelectedIndex >= 0 체크 3개소
- fom_Data: SelectedItem null 체크 2개소
- fomCurve: CurrentRow null 체크 2개소

##### 2.3 CrossThread 방어 (InvokeRequired + BeginInvoke)

| 파일 | 메서드 | 내용 |
|------|--------|------|
| fom_Main.cs | Indi_LogData | InvokeRequired + BeginInvoke 패턴 |
| fom_Main.cs | ABSB_LogData | 동일 |
| fom_Main.cs | PLC_Log_Data | 동일 |
| fom_Main.cs | ABSBScanHerz | 동일 |
| fom_Test.cs | Refresh_Data 외 6개 | 7개 메서드에 InvokeRequired 추가 |
| cls_ABSB.cs | ThreadABSB 외 | BeginInvoke + IsHandleCreated 4곳 |

##### 2.4 NullReference 방어
- clsDBSql.cs: DBs_Conn null 체크 (DS_Select, DT_Select, Execute, Create)

##### 2.5 빈 catch 블록 로그 추가 (36개)
- 전체 170개 catch 중 비어있는 36개에 `Logs.MakeLog_File(Log_His.Err_, ...)` 추가
- cls_Logs.cs ExceptionErr (재귀 방지용) 1개 제외

##### 2.6 ListBox 누적 방지
- lst_Logs, lst_PLCs, lst_ABSB, lst_Indi, lst_DLCs: 500개 초과 시 오래된 항목 자동 삭제
- 삭제된 로그는 파일(Log File.log)에 별도 기록됨

#### 3. 기타 수정
- **cboModel 드롭다운**: PLC Select 처리 시 `!cboModel.DroppedDown` 체크 추가
- **fomCurve 타이틀**: 새 커브 생성 시 타이틀바에 모델명 표시
- **icsNeoClass**: ConvertFromHex catch 타입 OverflowException 유지

#### 4. 사이드 이펙트 검증

| 항목 | 결과 |
|------|------|
| Invoke → BeginInvoke (4개 메서드) | 안전 |
| fom_Test InvokeRequired (7개 메서드) | 안전 |
| cls_ABSB BeginInvoke + IsHandleCreated | 안전 |
| clsDBSql null 체크 | 안전 |
| icsNeoClass catch 타입 | 복원 완료 |
| fomCurve TryParse | 낮은 위험 |
| fomSetup SelectedIndex 범위 체크 | 안전 |
| 빈 catch 로그 추가 (36개) | 안전 |
| ListBox 500개 제한 | 안전 |

#### 5. 현장 확인 필요 사항

| # | 항목 | 확인 방법 |
|---|------|-----------|
| 1 | 바코드 100대 후 미인식 해소 | ListBox 제한 적용 후 100대 이상 연속 검사 |
| 2 | cboModel 드롭다운 닫힘 해소 | 현장에서 드롭다운 수동 선택 시도 |
| 3 | 에러 로그 확인 | Log File.log의 Error 항목 확인 |

## 배포 시 확인 사항

| 항목 | 개발 PC | 현장 배포 |
|------|---------|-----------|
| fom_Main.cs:322 | ~~주석 처리~~ | ABSBoard.Setting() 주석 해제 (완료) |
| MachineSet.def:125 | ~~OnwerDrv=0~~ | OnwerDrv=1 (완료) |

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
