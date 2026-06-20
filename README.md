# Slot Container
Slot 기반 데이터 컨테이너로,  `Fixed` / `Compact` 레이아웃을 지원하며  슬롯 단위로 아이템을 스택 형태로 저장하고 관리할 수 있으며 
인벤토리, 장비 슬롯, 아이템 보관함 등  “슬롯 + 스택 구조”가 필요한 모든 시스템에 사용할 수 있도록 설계되었습니다.

<br>
아직 지속적인 프로젝트 사용 테스트가 필요합니다. <br>

<br>

[Usage](#usage) <br>
[API](#api)

<br>
<br>
<br>


## 🎞️Example
`Runtime/Exapmle`폴더에 용도에 맞게 상속이 필요한 경우, 예시 스크립트와 프리팹이 준비되어 있습니다. <br>
<img width="333" height="740" alt="컨테이너 example" src="https://github.com/user-attachments/assets/958f5f85-f784-41c9-b957-c3b2f9196c3e" /> <br>


<br>
<br>
<br>

## 🔧 Usage

`SlotContainer`는 Item + Stack 구조를 기반으로 동작하며,  
`ExampleContainer`처럼 상속하여 조회/캐싱 기능을 확장할 수 있습니다.

<br>

#### Item 정의

```csharp
public class ExampleItem : IItem<string>
{
    public string Key { get; }
    public string Name { get; }
    public int MaxStack { get; }

    public ExampleItem(string key, string name, int maxStack)
    {
        Key = key;
        Name = name;
        MaxStack = maxStack;
    }
}
```

<br>
<br>

#### Container 생성
```cs
var container = new SlotContainer(
    SlotContainer<ExampleItem, string>.SlotLayout.Fixed,
    capacity: 10
);

// capacity = -1 > 용량 제한 없음. 
```

<br>
<br>

#### 아이템 관리
```cs
//추가
var potion = new ExampleItem("potion", "Health Potion", 10);
container.Add(potion, 15);

//제거
container.RemoveAt(index: 0, amount: 3);

//초기화
container.Clear();
```

<br>
<br>
<br>

## 📖API
#### SlotContainer
**`Add(item, amount)`** : 아이템을 슬롯에 추가합니다, 실제로 추가된 수량을 반환합니다.<br>
**`RemoveAt(index, amount)`** : 지정된 슬롯 인덱스에서 아이템을 제거합니다, 실제로 제거된 수량을 반환합니다. <br>
**`GetSlot(index)`** : 슬롯 인덱스를 통해 해당 슬롯을 반환합니다. <br>
**`Clear()`** : 모든 슬롯을 초기화합니다. <br>
**`ConsumeDirty()`** : 변경된 슬롯 인덱스를 반환하고 dirty 상태를 초기화합니다. (UI/ 동기화용) <br>
**`IsSameItem(a,b)`** : 아이템 동일성을 비교합니다. 기본 구현은 Key 기반 비교입니다. <br>
**`OnSlotChanged(index, slot)`** : 슬롯 변경 시 호출되는 확장 훅입니다. 상속해서 UI / Indexing / Cache 갱신 등에 사용합니다. <br>

<br>

#### SlotLayout
**`Fixed`** : 슬롯 개수 고정, 빈 슬롯 유지, 인덱스 구조 유지 <br>
**`Compact`** : 빈 슬롯 제거, 슬롯 자동 압축, 동적 구조 <br>

<br>

#### ItemStack
**`Item`** : 슬롯에 저장된 아이템 <br>
**`Count`** : 현재 스택 수량 <br>
**`IsEmpty`** : 슬롯이 비어있는지 여부 <br>
**`Add(amount)`** : 슬롯에 수량 추가 <br>
**`Remove(amount)`** : 슬롯에서 수량 제거 <br>
**`Set(item, amount)`** : 슬롯에 아이템 및 수량 설정 <br>
**`Clear()`** : 슬롯 초기화 <br>
