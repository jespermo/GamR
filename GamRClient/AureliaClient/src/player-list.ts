import {inject} from 'aurelia-framework';
import {Api} from './api';

@inject(Api)
export class PlayerList {
  players;
  selectedId = 0;

  constructor(private api: Api) { }

  created() {
     this.api.getPlayersList().then(players => {
         this.players = players
     });
  }

  select(player) {
    this.selectedId = player.id;
    return true;
  }
}