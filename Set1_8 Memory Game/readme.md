## Memory Game

메모리 게임 내에 등장하는 도형을 그리는 메소드를 정의한다.

그리고 그 메소드를 이용하여 다양한 도형을 만드는 함수를 정의한다.

Command Bar의 New버튼을 누를 시에 Layout 메소드를 통해 보드판을 설정하고 Select 메소드를 통해 랜덤한 숫자를 선택하여 이전에 정의한 도형들과 매칭시킨다.

Layout메소드에서는 보드판을 리셋시키고 새로운 보드판을 세팅한다. 이 과정에서 Add메소드를 호출한다.

Add메소드에서는 보드판에 들어가는 캔버스를 세팅하고 사용자의 터치를 파악하여 같은 도형을 클릭 했을떄와 다른 도형을 클릭했을떄를 파악한다. 사용자가 전부 맞췄을 떄는 총 몇 번의 터치로 클리어했는지를 MessageDialog로 띄워준다.