# KeySoundChan

키보드 입력 시 사운드와 이미지를 재생하는 WPF 애플리케이션.

## 기술 스택

- **.NET 8 + WPF** - UI 프레임워크
- **NAudio** - 저지연 오디오 재생 (WASAPI)
- **CommunityToolkit.Mvvm** - MVVM 패턴
- **Hardcodet.NotifyIcon.Wpf** - 시스템 트레이

## 프로젝트 구조

```
KeySoundChan/
├── App.xaml                    # 앱 진입점, 전역 리소스
├── MainWindow.xaml/.cs         # 메인 윈도우 (설정 UI)
├── MiniWindow.xaml/.cs         # 미니 모드 윈도우 (투명, OBS 캡처용)
├── Converters/
│   └── BoolToVisibilityConverter.cs
├── Services/
│   ├── AudioService.cs         # NAudio 기반 오디오 재생
│   └── KeyboardHookService.cs  # 전역 키보드 훅
├── ViewModels/
│   └── MainViewModel.cs        # 메인 뷰모델
└── resources/                  # 사운드 프리셋, 아이콘
```

## 빌드 및 실행

```bash
dotnet build           # 빌드
dotnet run             # 실행
dotnet build -c Release  # 릴리스 빌드
```

## 주요 기능

1. **키 사운드 재생** - 키 누를 때/뗄 때 사운드 재생
2. **이미지 애니메이션** - 키 입력마다 이미지 프레임 전환 (GIF/다중 이미지)
3. **미니 모드** - 투명 배경으로 이미지만 표시 (OBS 윈도우 캡처용)
4. **시스템 트레이** - 최소화 시 트레이로 이동

## 미니 모드

- 메인 창 숨기고 별도 투명 윈도우 표시
- **좌클릭 드래그**: 창 이동
- **우클릭**: 미니 모드 종료
- 항상 위에 표시 (`Topmost`)

## 설정 저장 경로

- `%LocalAppData%/KeySoundChan/settings.txt` - 앱 설정
- `%LocalAppData%/KeySoundChan/window.txt` - 창 크기

