import { PlayroomModule } from './playroom.module';

describe('PlayroomModule', () => {
  let playroomModule: PlayroomModule;

  beforeEach(() => {
    playroomModule = new PlayroomModule();
  });

  it('should create an instance', () => {
    expect(playroomModule).toBeTruthy();
  });
});
