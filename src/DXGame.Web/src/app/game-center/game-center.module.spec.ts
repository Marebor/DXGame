import { GameCenterModule } from './game-center.module';

describe('GameCenterModule', () => {
  let gameCenterModule: GameCenterModule;

  beforeEach(() => {
    gameCenterModule = new GameCenterModule();
  });

  it('should create an instance', () => {
    expect(gameCenterModule).toBeTruthy();
  });
});
