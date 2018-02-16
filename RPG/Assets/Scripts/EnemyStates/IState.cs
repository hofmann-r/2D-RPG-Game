using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {

    //prepara o estado
    void Enter(Enemy parent);

    //atualiza o estado
    void Update();

    //termina o estado
    void Exit();
}
